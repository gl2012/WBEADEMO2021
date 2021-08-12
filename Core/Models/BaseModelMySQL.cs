using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
namespace WBEADMS.Models
{
    public abstract class BaseModelMySQL : IComparable
    {
        protected static Dictionary<string, Dictionary<string, string>> _tableMetaData = new Dictionary<string, Dictionary<string, string>>();

        protected readonly string _connectionString;

        protected readonly string[] _fields;
        protected readonly string _tableName;
        protected readonly string _primaryKey;
        protected NameValueCollection _originalData;
        protected NameValueCollection _data;
        protected Hashtable _relatedObjects; // _relatedObjects[_data field] = foreign BaseModel or array of BaseModel; use LoadRelated or LoadRelatedJoin to populate
        private Dictionary<string, List<string>> _relatedIds;  // _relatedIds[TModel name] = CSV of Ids; use AddRelatedList, SetRelatedList, RemoveRelatedList update items and GetRelatedList to get items  // TODO: move relatedIds and Join management methods to it's own class to shorten BaseModel and avoid the generic in generic type.

        public string id { get { return _data[_primaryKey]; } private set { _data[_primaryKey] = value; } }

        protected bool IsNewRecord { get { return _data[_primaryKey].IsBlank(); } }


        protected BaseModelMySQL() : this(null, null, null) { }

        protected BaseModelMySQL(string[] fields) : this(null /* tableName */, null /* primaryKey */, fields) { }

        protected BaseModelMySQL(string tableName, string primaryKey, string[] fields)
        {
            _connectionString = ModelsCommon.FetchMysqlConnectionString();
            _originalData = new NameValueCollection();
            _data = new NameValueCollection();
            _relatedObjects = new Hashtable();
            _relatedIds = new Dictionary<string, List<string>>();

            _tableName = tableName ?? TableName(this.GetType());
            _primaryKey = primaryKey ?? TableIdField(this.GetType());

            if (fields == null)
            {
                Dictionary<string, string> fieldSet = GetTableMetaData();
                _fields = new string[fieldSet.Count];
                fieldSet.Keys.CopyTo(_fields, 0);
            }
            else
            {
                _fields = fields;
            }
        }

        #region save methods
        public abstract void Validate();

        /// <summary>Default Save method. Override if primary key is a Guid or special handling is required.</summary>
        public virtual void Save()
        {
            Validate();
            if (IsNewRecord)
            {
                object newId = SaveNew();
                id = newId.ToString();
            }
            else
            {
                SaveEdits();
            }
        }

        protected List<string> GetChangedFields()
        {
            Dictionary<string, string> fieldTypes = GetTableMetaData();

            List<string> changedFields = new List<string>();
            foreach (string field in _data.AllKeys)
            {
                // add to list if string fields do not match
                if (!DoDataFieldsMatch(_data[field], _originalData[field], fieldTypes[field]))
                {
                    changedFields.Add(field);
                }
            }

            return changedFields;
        }

        private Dictionary<string, string> GetTableMetaData()
        {
            if (_tableMetaData.ContainsKey(_tableName))
            {
                return _tableMetaData[_tableName];
            }

            Dictionary<string, string> fieldData = new Dictionary<string, string>();
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                /* string sql =
                     @"SELECT c.name, t.name as type
                       FROM sys.columns AS c
                       JOIN sys.types AS t ON c.user_type_id=t.user_type_id
                       Where c.object_id = OBJECT_ID(@tableName)";*/

                string sql = @"select column_name as name, column_type as type from information_schema.columns where table_name=@tableName";

                using (MySqlCommand loadCommand = new MySqlCommand(sql, connection))
                {
                    loadCommand.Parameters.AddWithValue("tableName", _tableName);
                    using (MySqlDataReader dataReader = loadCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            fieldData.Add((string)dataReader[0], (string)dataReader[1]);
                        }
                    }
                }
            }

            // cache the field data.
            _tableMetaData.Add(_tableName, fieldData);

            // return the data to the caller.
            return fieldData;
        }

        private bool DoDataFieldsMatch(string value1, string value2, string dataType)
        {
            // if the strings match the just get out.
            // this covers "" == "" and "x" == "x" cases.
            if (value1 == value2)
            {
                return true;
            }

            // if only one has a value.
            // this covers "" == "x" or "x" == ""
            if (value1.IsBlank() != value2.IsBlank())
            {
                return false;
            }

            if (value1 == null || value2 == null) return false;

            // this covers "x" == "y" where y might be a different format of x and vice versa
            switch (dataType)
            {
                // TODO: use int.Parse instead of TryParse since throwing exceptions is acceptable behaviour
                case "int":
                    int i1, i2;
                    if (int.TryParse(value1, out i1))
                    {
                        if (int.TryParse(value2, out i2))
                        {
                            return i1 == i2;
                        }
                    }
                    break;
                case "datetime":
                    DateTime dt1, dt2;
                    if (DateTime.TryParse(value1, out dt1))
                    {
                        if (DateTime.TryParse(value2, out dt2))
                        {
                            return dt1 == dt2;
                        }
                    }
                    break;
                case "bigint":
                    long l1, l2;
                    if (long.TryParse(value1, out l1))
                    {
                        if (long.TryParse(value2, out l2))
                        {
                            return l1 == l2;
                        }
                    }
                    break;
                case "decimal":
                case "float":
                    decimal d1, d2;
                    if (decimal.TryParse(value1, out d1))
                    {
                        if (decimal.TryParse(value2, out d2))
                        {
                            return d1 == d2;
                        }
                    }
                    break;
                case "bit":
                    return value1.ToLower() == value2.ToLower();
            }
            return false;
        }

        public bool IsModified()
        { // TODO: put this in BaseModel.Save() to skip save if no fields has changed

            // if the number of elements in _data has changed, then there has been a modification.
            if (_originalData.Count != _data.Count)
            {
                return true;
            }

            foreach (string field in _fields)
            {
                if (_originalData[field] != _data[field])
                {
                    return true;
                }
            }

            return false;
        }

        protected string GetInsertStatement(List<string> fields)
        {
            if (fields.Count == 0)
            {
                //if there are now values associated then use the default values.
                return "INSERT INTO " + _tableName + " DEFAULT VALUES; SELECT SCOPE_IDENTITY()";
            }

            if (IsNewRecord)
            {
                // don't set primary key if it is empty i.e. where primary key is autonumber; primary key would not be empty where it must be specified, i.e. GUID fields
                fields.Remove(_primaryKey);
            }

            var colNames = String.Join(",", fields.ToArray());
            var paramNames = "@" + String.Join(",@", fields.ToArray());
            var identSelect = "SELECT SCOPE_IDENTITY()"; // NOTE: this function requires SQL Server 2000; which limits by scope. Pre-2000, we must use @@IDENTITY which does not limit by scope.
            return "INSERT INTO " + _tableName + " ( " + colNames + ") VALUES (" + paramNames + ");" + identSelect;
        }

        protected string GetUpdateStatement(List<string> fields)
        {
            if (fields.Count == 0) { throw new ArgumentException("Unable to generate SQL to update " + _tableName + " because list of changed fields is empty."); }
            var setPairs = new List<string>();
            foreach (string col in fields) { setPairs.Add(col + "=@" + col); }
            var setClause = "SET " + String.Join(",", setPairs.ToArray());

            return "UPDATE " + _tableName + " " + setClause + " WHERE " + _primaryKey + "=@" + _primaryKey;
        }

        /// <summary>Saves a new instance of the model into the database.</summary>
        /// <returns>Returns the primary key of the newly saved row.</returns>
        protected virtual object SaveNew()
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var changedFields = GetChangedFields();
                string insertSQL = GetInsertStatement(changedFields);

                using (MySqlCommand insertCommand = new MySqlCommand(insertSQL, connection))
                {
                    foreach (string field in changedFields)
                    {
                        if (_data[field].IsBlank())
                        {
                            insertCommand.Parameters.AddWithValue(field, DBNull.Value);
                        }
                        else
                        {
                            insertCommand.Parameters.AddWithValue(field, (object)_data[field].Trim());
                        }
                    }

                    return insertCommand.ExecuteScalar();
                }
            }
        }

        protected virtual void SaveEdits()
        {
            // WARNING: This method should never update the original data it will break Auditing
            // it would be needless to update the original data as it will be distroyed shortly after this function call finishes.
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var changedFields = GetChangedFields();

                // no changes have been made exit.
                if (changedFields.Count <= 0)
                {
                    return;
                }

                string updateSQL = GetUpdateStatement(changedFields);

                using (MySqlCommand updateCommand = new MySqlCommand(updateSQL, connection))
                {
                    updateCommand.Parameters.AddWithValue(_primaryKey, _data[_primaryKey]);

                    foreach (string field in changedFields)
                    {
                        if (_data[field].IsBlank())
                        {
                            updateCommand.Parameters.AddWithValue(field, DBNull.Value);
                        }
                        else
                        {
                            updateCommand.Parameters.AddWithValue(field, (object)_data[field].Trim());
                        }
                    }

                    updateCommand.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region join table methods

        private List<string> GetRelatedListDB<TModel>(string joinTable, string relatedIdField, string subjectIdField, string whereClause)
        {
            var list = new List<string>();
            if (id == null) { return list; }

            // TODO: move this copy & pasted block to it's own function for defaulting table & column names
            if (joinTable.IsBlank()) { joinTable = _tableName + "_" + TableName(typeof(TModel)); }
            if (relatedIdField.IsBlank()) { relatedIdField = TableIdField(typeof(TModel)); }
            if (subjectIdField.IsBlank()) { subjectIdField = _primaryKey; }

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string sql =
                    "SELECT " + relatedIdField +
                    " FROM " + joinTable +
                    " WHERE " + subjectIdField + " = " + id;
                if (!whereClause.IsBlank())
                {
                    sql += " AND " + whereClause; // TODO: make this whereClause into an object so you can pass in ORDER BY clauses as well
                }

                using (MySqlCommand loadCommand = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader dataReader = loadCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            string childId = dataReader[relatedIdField].ToString();
                            list.Add(childId);
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>Loads a list of related table IDs and caches.  (Note this does not load the Models)</summary>
        protected List<string> GetRelatedList<TModel>() where TModel : BaseModel, new()
        {
            string key = TypeName(typeof(TModel));
            if (_relatedIds.ContainsKey(key))
            {
                return _relatedIds[key];
            }
            else
            {
                _relatedIds[key] = GetRelatedListDB<TModel>(null /* joinTable */, null /* relatedIdField */, null /* subjectIdField */, null /* whereClause */);
                return _relatedIds[key];
            }
        }

        /// <summary>Adds a related join model ID.</summary>
        protected void AddToRelatedList<TModel>(string id) where TModel : BaseModel, new()
        { // TODO: this is poor use of generics: as a way to pass in the model name; however, this maintains consistency with other generics where the model name and class is both used.
            string key = TypeName(typeof(TModel));
            var list = GetRelatedList<TModel>();
            list.Add(id);
            _relatedIds[key] = list;
        }

        /// <summary>Removes a related join model ID.</summary>
        protected void RemoveFromRelatedList<TModel>(string id) where TModel : BaseModel, new()
        { // TODO: this is poor use of generics: as a way to pass in the model name; however, this maintains consistency with other generics where the model name and class is both used.
            string key = TypeName(typeof(TModel));
            var list = GetRelatedList<TModel>();
            list.Remove(id);
            _relatedIds[key] = list;
        }

        /// <summary>Sets all related join model ID.</summary>
        /// <param name="list">Comma seperated value list of related join model IDs.</param>
        protected void SetRelatedList<TModel>(string list)
        { // TODO: this is poor use of generics: as a way to pass in the model name; however, this maintains consistency with other generics where the model name and class is both used.
            string key = TypeName(typeof(TModel));
            if (list.IsBlank())
            {
                _relatedIds[key] = new List<string>();
            }
            else
            {
                _relatedIds[key] = new List<string>(list.Split(','));
            }
        }

        /// <summary>Saves join table.  Set the IDs with AddToRelatedList() or SetRelatedList().</summary>
        protected void SaveRelatedJoin<TModel>() where TModel : BaseModel, new()
        {
            SaveRelatedJoin<TModel>(null /* joinTable */, null /* relatedIdField */, null /* subjectIdField */);
        }

        /// <summary>Saves join table.  Set the IDs with AddToRelatedList() or SetRelatedList().</summary>
        protected void SaveRelatedJoin<TModel>(string joinTable) where TModel : BaseModel, new()
        {
            SaveRelatedJoin<TModel>(joinTable, null /* relatedIdField */, null /* subjectIdField */);
        }

        /// <summary>Saves join table.  Set the IDs with AddToRelatedList() or SetRelatedList().</summary>
        protected void SaveRelatedJoin<TModel>(string relatedIdField, string subjectIdField) where TModel : BaseModel, new()
        {
            SaveRelatedJoin<TModel>(null /* joinTable */, relatedIdField, subjectIdField);
        }

        /// <summary>Saves join table.  Set the IDs with AddToRelatedList() or SetRelatedList().</summary>
        protected void SaveRelatedJoin<TModel>(string joinTable, string relatedIdField, string subjectIdField) where TModel : BaseModel, new()
        {
            if (!_relatedIds.ContainsKey(TypeName(typeof(TModel)))) { return; } // do nothing if relatedIds is untouched

            // TODO: move this copy & pasted block to it's own function for defaulting table & column names
            if (joinTable.IsBlank()) { joinTable = _tableName + "_" + TableName(typeof(TModel)); }
            if (relatedIdField.IsBlank()) { relatedIdField = TableIdField(typeof(TModel)); }
            if (subjectIdField.IsBlank()) { subjectIdField = _primaryKey; }

            // init
            var existingItems = GetRelatedListDB<TModel>(joinTable, relatedIdField, subjectIdField, null /* whereClause */);
            var updatedItems = GetRelatedList<TModel>();

            string sql = String.Empty;

            // determine which related items to remove, which to insert, and which to leave as is
            foreach (var existingItem in existingItems)
            {
                // remove items that already exist in the database.
                if (updatedItems.Contains(existingItem))
                {
                    updatedItems.Remove(existingItem);
                }
                else
                { // remove old items that have been removed by the user. does not contain existing item.
                    // remove item from list.
                    sql += String.Format("DELETE FROM {0} WHERE {1}='{3}' AND {2}='{4}';",
                            joinTable, subjectIdField, relatedIdField, id, existingItem);
                }
            }

            // generate SQL statement // TODO: do we need to escape single quotes in IDs?  doubling the singlequotes will do for TSQL.
            foreach (var related_id in updatedItems)
            {
                sql +=
                    String.Format("INSERT INTO {0} ({1}, {2}) VALUES ('{3}','{4}');",
                    joinTable, subjectIdField, relatedIdField, id, related_id);
            }

            if (sql.Length != 0)
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (MySqlCommand insertCommand = new MySqlCommand(sql, connection))
                    {
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        #endregion

        #region load methods
        /// <summary>Loads a model given an id or returns null if unable to do so.</summary>
        /// <typeparam name="TModel">Model class type</typeparam>
        /// <param name="id">id of the model</param>
        internal static TModel Load<TModel>(string id) where TModel : BaseModelMySQL, new()
        {
            try
            {

                // Test to see if it is an integer if not return null. This prevents an SqlException when it tries to cast string to int and it can't.
                // If the id is blank IsInt() returns false;
                if (!id.IsInt())
                {
                    return null;
                }

                TModel item = new TModel();
                item._data[item._primaryKey] = id;
                item.LoadData();
                return item;
            }
            catch (ArgumentException)
            {
                // TODO: error log failed loads?  should we, since the catch buries exceptions? or shouldn't we, since those are likely bad user urls
                // TODO: testing is required since I changed this catch all to only catch ArgumentException (which occurs on id not found in DB); did location model throw generic exceptions?
                return null; // return null 
            }
        }

        /// <summary>Loads model based on _primaryKey as defined by default constructor.</summary>
        protected virtual void LoadData()
        {
            LoadData(_primaryKey);
        }

        /// <summary>Loads model based on field.</summary>
        /// <param name="idField">field to check for; the value set in _data[idField] is used.</param>
        protected virtual void LoadData(string idField)
        {
            if (string.IsNullOrEmpty(_data[idField]))
            {
                throw new ArgumentException("ID must be provided to load data.");
            }

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand selectCommand = new MySqlCommand(@"
                        SELECT * 
                        FROM " + _tableName +
                      " WHERE " + idField + " = @id", connection))
                {

                    selectCommand.Parameters.AddWithValue("tableName", _tableName);
                    selectCommand.Parameters.AddWithValue("id", _data[idField]);

                    using (MySqlDataReader dataReader = selectCommand.ExecuteReader())
                    {
                        if (!dataReader.Read())
                        {
                            throw new ArgumentException("Unable to find data for Id (" + _data[idField] + ")");
                        }

                        Dictionary<string, string> tableMetaData = GetTableMetaData();

                        string fieldValue;
                        foreach (string field in _fields)
                        {
                            fieldValue = dataReader[field].ToString();

                            switch (tableMetaData[field])
                            {
                                case "datetime":
                                    fieldValue = fieldValue.ToDateTimeFormat();
                                    break;
                                case "varbinary":
                                    fieldValue = System.Text.Encoding.Default.GetString((byte[])dataReader[field]);
                                    break;
                                case "decimal":
                                case "float":
                                    fieldValue = fieldValue.ToDecimalFormat();
                                    break;

                            }
                            _originalData[field] = fieldValue;
                            _data[field] = _originalData[field];
                        }
                    }
                }
            }
        }

        /// <summary>Loads a related model and caches.</summary>
        /// <typeparam name="TModel">related model type</typeparam>
        protected TModel LoadRelated<TModel>() where TModel : BaseModelMySQL, new()
        {
            string key = TableIdField(typeof(TModel));
            return LoadRelated<TModel>(key);
        }

        /// <summary>Loads a related model and caches.</summary>
        /// <typeparam name="TModel">related model type</typeparam>
        /// <param name="key">The foreign key field for the related model stored as _data[key]</param>
        protected TModel LoadRelated<TModel>(string key) where TModel : BaseModelMySQL, new()
        {
            if (_relatedObjects[key] == null && !String.IsNullOrEmpty(_data[key]))
            {
                _relatedObjects[key] = Load<TModel>(_data[key]);
            }

            return (TModel)_relatedObjects[key];
        }

        /// <summary>Loads an array of related models and caches.  
        /// i.e. mylocation.LoadByForeignKey&lt;Note&gt;() will load all rows from Notes table where location_id = mylocation.id</summary>
        /// <typeparam name="TModel">related model type</typeparam>
        ////protected TModel[] LoadByForeignKey<TModel>() where TModel : BaseModel, new() {
        ////     return LoadByForeignKey<TModel>(null /* key */);
        ////}

        /// <summary>Loads an array of related models and caches.  
        /// i.e. myuser.LoadByForeignKey&lt;Location&gt;("created_by") will load all rows from Locations table where created_by = myuser.id</summary>
        /// <typeparam name="TModel">related model type</typeparam>
        /// <param name="key">Foreign key used, i.e. created_by on Locations table</param>
        /*
        protected TModel[] LoadByForeignKey<TModel>(string key) where TModel : BaseModel, new() {
            if (key.IsBlank()) { key = _primaryKey; }
            string objIndex = TableName(typeof(TModel)) + "_" + key;
            if (_relatedObjects[objIndex] == null && !id.IsBlank()) {
                _relatedObjects[objIndex] = FetchAll<TModel>(key + "=" + id).ToArray();
            }
            return (TModel[])_relatedObjects[objIndex];
        }
        */

        /// <summary>Loads an array of paginated related models and caches.
        /// i.e. myuser.LoadByForeignKey&lt;Location&gt;() will load all rows from Locations table where created_by = myuser.id</summary>
        /// <typeparam name="TModel">related model type</typeparam>
        protected TModel[] LoadByForeignKey<TModel>() where TModel : BaseModelMySQL, new()
        {
            return LoadByForeignKey<TModel>(_primaryKey);
        }

        /// <summary>Loads an array of paginated related models and caches.
        /// i.e. myuser.LoadByForeignKey&lt;Location&gt;("created_by") will load all rows from Locations table where created_by = myuser.id</summary>
        /// <typeparam name="TModel">related model type</typeparam>
        /// <param name="key">name of primary key</param>
        protected TModel[] LoadByForeignKey<TModel>(string key) where TModel : BaseModelMySQL, new()
        {
            if (key.IsBlank()) { key = _primaryKey; }
            string objIndex = TableName(typeof(TModel)) + "_" + key;
            if (_relatedObjects[objIndex] == null && !IsNewRecord)
            {
                _relatedObjects[objIndex] = FetchAll<TModel>(new { where = key + "=" + id }).ToArray();
            }

            return (TModel[])_relatedObjects[objIndex];
        }

        /// <summary>Loads an array of paginated related models and caches.
        /// i.e. myuser.LoadByForeignKey&lt;Location&gt;("created_by") will load all rows from Locations table where created_by = myuser.id</summary>
        /// <typeparam name="TModel">related model type</typeparam>
        /// <param name="page">page number of paginated data set</param>
        /// <param name="pageSize">page size of paginated data set</param>
        protected TModel[] LoadByForeignKey<TModel>(int page, int pageSize) where TModel : BaseModelMySQL, new()
        {
            return LoadByForeignKey<TModel>(null /* key */, page, pageSize);
        }

        /// <summary>Loads an array of related models and caches.  
        /// i.e. myuser.LoadByForeignKey&lt;Location&gt;("created_by") will load all rows from Locations table where created_by = myuser.id</summary>
        /// <typeparam name="TModel">related model type</typeparam>
        /// <param name="key">Foreign key used, i.e. created_by on Locations table</param>
        /// <param name="page">page number of paginated data set</param>
        /// <param name="pageSize">page size of paginated data set</param>
        protected TModel[] LoadByForeignKey<TModel>(string key, int page, int pageSize) where TModel : BaseModelMySQL, new()
        {
            if (key.IsBlank()) { key = _primaryKey; }
            string objIndex = TableName(typeof(TModel)) + "_" + key;
            if (_relatedObjects[objIndex] == null && !IsNewRecord)
            {
                _relatedObjects[objIndex] = FetchPage<TModel>(page, pageSize, new { where = key + "=" + id }).ToArray();
            }

            return (TModel[])_relatedObjects[objIndex];
        }

        /// <summary>Loads a list of related models and caches.
        /// If the subject model is Parent and related model is Child, 
        /// then the join table will be Parents_Childs, with columns of parent_id and child_id.</summary>
        /// <typeparam name="TModel">related model type, i.e. Child.</typeparam>
        protected TModel[] LoadRelatedJoin<TModel>() where TModel : BaseModelMySQL, new()
        {
            return LoadRelatedJoin<TModel>(null /* joinTable */, null /* relatedIdField */, null /* subjectIdField */);
        }

        /// <summary>Loads a list of related models and caches.
        /// If the subject model is Subject and related model is Related,
        /// The columns will be subject_id and child_id.</summary>
        /// <typeparam name="TModel">related model type, i.e. Related.  The related column ids will be inferred.</typeparam>
        /// <param name="joinTable">The join table, i.e. Relateds_Subjects or Subjects_Relateds, given that the models are Related and Subject.</param>
        protected TModel[] LoadRelatedJoin<TModel>(string joinTable) where TModel : BaseModelMySQL, new()
        {
            return LoadRelatedJoin<TModel>(joinTable, null /* relatedIdField */, null /* subjectIdField */);
        }

        /// <summary>Loads a list of related models and caches.
        /// If both subject and related models are Note,
        /// the joinTable will be Notes_Notes, and the columns must be specified as to 
        /// which is the subject_id field and which is related_id field to load related models.</summary>
        /// <typeparam name="TModel">related model type, i.e. Note</typeparam>
        /// <param name="relatedIdField">The key field for related, i.e. child_note_id in Notes_Notes table</param>
        /// <param name="subjectIdField">The key field for the subject and is used in the WHERE clause with the subject_id, i.e. parent_note_id in Notes_Notes table</param>
        protected TModel[] LoadRelatedJoin<TModel>(string relatedIdField, string subjectIdField) where TModel : BaseModelMySQL, new()
        {
            return LoadRelatedJoin<TModel>(null /* joinTable */, relatedIdField, subjectIdField);
        }

        /// <summary>Loads a list of related models and caches.</summary>
        /// <typeparam name="TModel">related model type, i.e. Child</typeparam>
        /// <param name="joinTable">The join table, i.e. Parents_Childs, if the parent model is Parent</param>
        /// <param name="relatedIdField">The key field for related model, i.e. child_id in Parents_Childs</param>
        /// <param name="subjectIdField">The key field for subject model, i.e. parent_id in Parents_Childs</param>
        protected TModel[] LoadRelatedJoin<TModel>(string joinTable, string relatedIdField, string subjectIdField) where TModel : BaseModelMySQL, new()
        {
            return LoadRelatedJoin<TModel>(joinTable, relatedIdField, subjectIdField, null);
        }

        /// <summary>Loads a list of related models and caches.</summary>
        /// <typeparam name="TModel">related model type, i.e. Child</typeparam>
        /// <param name="joinTable">The join table, i.e. Parents_Childs, if the parent model is Parent</param>
        /// <param name="relatedIdField">The key field for related model, i.e. child_id in Parents_Childs</param>
        /// <param name="subjectIdField">The key field for subject model, i.e. parent_id in Parents_Childs</param>
        /// <param name="whereClause"></param>
        protected TModel[] LoadRelatedJoin<TModel>(string joinTable, string relatedIdField, string subjectIdField, string whereClause) where TModel : BaseModelMySQL, new()
        {
            if (IsNewRecord) { return new TModel[] { }; }

            // defaults
            if (joinTable.IsBlank()) { joinTable = _tableName + "_" + TableName(typeof(TModel)); }
            if (relatedIdField.IsBlank()) { relatedIdField = TableIdField(typeof(TModel)); }
            if (subjectIdField.IsBlank()) { subjectIdField = _primaryKey; }

            if (_relatedObjects[relatedIdField] == null)
            {

                var list = new List<TModel>();
                foreach (string childId in GetRelatedListDB<TModel>(joinTable, relatedIdField, subjectIdField, whereClause))
                {
                    list.Add(Load<TModel>(childId));
                }

                _relatedObjects[relatedIdField] = list.ToArray();
            }

            return (TModel[])_relatedObjects[relatedIdField];  // TODO: low priority: pass back by deep copy, instead of pass back back reference.  There is danger of this value being modified; however, since we don't save these record, it should not be a problem. --James 2009-10-16
        }
        #endregion

        #region fetch and pagination methods
        // TODO: make these FetchDictionary methods private; and implement FetchSelectList if all models need to implement FetchSelectList anyways

        /// <summary>Returns dictionary containing key/value pairs from the database table.</summary>
        /// <param name="value">database table column name representing the content for a html select option</param>
        protected static Dictionary<string, string> FetchDictionary<TModel>(string value)
        {
            string tableName = TableName(typeof(TModel));
            string key = TableIdField(typeof(TModel));
            return FetchDictionary(tableName, key, value, null /* whereClause */);
        }

        /// <summary>Returns dictionary containing key/value pairs from the database table.</summary>
        /// <param name="tableName">database table</param>
        /// <param name="key">database table column name representing the value attribute for a html select option</param>
        /// <param name="value">database table column name representing the content for a html select option</param>
        protected static Dictionary<string, string> FetchDictionary(string tableName, string key, string value)
        {
            return FetchDictionary(tableName, key, value, null /* whereClause */);
        }

        /// <summary>Returns dictionary containing key/value pairs from the database table.</summary>
        /// <param name="tableName">database table</param>
        /// <param name="key">database table column name representing the value attribute for a html select option</param>
        /// <param name="value">database table column name representing the content for a html select option</param>
        /// <param name="whereClause">where clause</param>
        protected static Dictionary<string, string> FetchDictionary(string tableName, string key, string value, string whereClause)
        {
            var fetch = new Dictionary<string, string>();
            using (MySqlConnection connection = new MySqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();
                string sql = "SELECT " + key + "," + value + " FROM " + tableName;

                // NOTE: does not handle where clauses made of white space.
                if (!String.IsNullOrEmpty(whereClause))
                {
                    sql += " WHERE " + whereClause;
                }

                using (MySqlCommand selectCommand = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader dataReader = selectCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            fetch.Add(dataReader[key].ToString(), dataReader[value].ToString()); // NOTE: this may error if key is not a database primary key column and returns the same key value as another row
                        }
                    }
                }
            }

            return fetch;
        }

        /// <summary>Fetch a single row based on a column.  Same as Load except you specify column.</summary>
        /// <param name="column">The column containing the row with the value you want to look for.</param>
        /// <param name="value">The value in the column you will find a unique row for.</param>
        public static TModel Fetch<TModel>(string column, string value) where TModel : BaseModelMySQL, new()
        {
            var model = new TModel();
            model._data[column] = value;
            try
            {
                model.LoadData(column);
            }
            catch (ArgumentException)
            {
                model = null;
            }

            return model;
        }

        /// <summary>Fetch all. The table/idField names assumed from TModel</summary>
        /// <typeparam name="TModel">Model class type; table/idField names assumed from TModel.</typeparam>
        public static List<TModel> FetchAll<TModel>() where TModel : BaseModelMySQL, new()
        {
            return FetchAll<TModel>(null /* tableName */, null /*idField */, null /*whereClause*/, null /* orderByClause */);
        }

        // TODO: make FetchAll/FetchPage protected to force the model to implement it's own FetchAll for the controller
        // TODO: move FetchAll/FetchPage/TotalCount/etc to a separate file so that it can be reached by Common.GetPaginatedListOrAddNotice

        /// <summary>Fetch all with WHERE clause. The table/idField names assumed from TModel. Note: the where clause must be scrubbed for sql injection beforehand.</summary>
        /// <typeparam name="TModel">Model class type; table/idField names assumed from TModel.</typeparam>
        /// <param name="whereClause">where clause inserted after WHERE in the sql query.</param>
        public static List<TModel> FetchAll<TModel>(string whereClause) where TModel : BaseModelMySQL, new()
        {
            return FetchAll<TModel>(null /* tableName */, null /*idField */, whereClause, null /* orderByClause */);
        }

        /// <summary>Fetch all with specified clauses. The table/idField names assumed from TModel.  Note: all the clauses must be scrubbed for sql injection beforehand.</summary>
        /// <param name="clauses">new object containing where, order, and join properties specifying the clauses</param>
        public static List<TModel> FetchAll<TModel>(object clauses) where TModel : BaseModelMySQL, new()
        {
            NameValueCollection dict = ObjectToDictionary(clauses);
            return FetchAll<TModel>(null /* tableName */, null /* idField */, dict["where"], dict["order"], dict["join"]);
        }

        /// <summary>Use FetchAll&lt;TModel&gt;(object clauses) instead. Fetch all with WHERE clause and ORDER BY clause. The table/idField names assumed from TModel.  Note: both clauses must be scrubbed for sql injection beforehand.</summary>
        /// <param name="whereClause">optional whereClause; null if not required.</param>
        /// <param name="orderByClause">order by clause inserted after ORDER BY in sql query.</param>
        [Obsolete("use FetchAll<TModel>(object clauses) instead", true)]  // TODO: implement FetchAll<TModel>(string field,string value), where this generates a safe way of doing whereClause of field=value by using parameterization
        public static List<TModel> FetchAll<TModel>(string whereClause, string orderByClause) where TModel : BaseModelMySQL, new()
        {
            return FetchAll<TModel>(null /* tableName */, null /* idField */, whereClause, orderByClause);
        }

        /// <summary>Fetch all with WHERE clause and specified tableName/idField.  Note: the where clause must be scrubbed for sql injection beforehand.</summary>
        /// <param name="tableName">table containing models</param>
        /// <param name="idField">key for database table</param>
        /// <param name="whereClause">where clause inserted after WHERE in the sql query.</param>
        public static List<TModel> FetchAll<TModel>(string tableName, string idField, string whereClause) where TModel : BaseModelMySQL, new()
        {
            return FetchAll<TModel>(tableName, idField, whereClause, null /* orderByClause */);
        }

        /// <summary>Fetch all of model with specified table/idField and where/order clauses.  Note: all the where clauses must be scrubbed for sql injection beforehand.</summary>
        /// <param name="tableName">table containing models</param>
        /// <param name="idField">key for database table</param>
        /// <param name="whereClause">where clause inserted after WHERE in the sql query.</param>
        /// <param name="orderByClause">order by clause inserted after ORDER BY in sql query.</param>
        public static List<TModel> FetchAll<TModel>(string tableName, string idField, string whereClause, string orderByClause) where TModel : BaseModelMySQL, new()
        {
            return FetchAll<TModel>(tableName, idField, whereClause, orderByClause, null /* joinClause */);
        }

        /// <summary>Fetch all of model with specified table/idField and where/order clauses. Note: all the where clauses must be scrubbed for sql injection beforehand.</summary>
        /// <param name="tableName">table containing models</param>
        /// <param name="idField">key for database table</param>
        /// <param name="whereClause">where clause inserted after WHERE in the sql query.</param>
        /// <param name="orderByClause">order by clause inserted after ORDER BY in sql query.</param>
        /// <param name="joinClause">join clause inserted after the FROM clause; i.e. INNER JOIN table1 </param>
        public static List<TModel> FetchAll<TModel>(string tableName, string idField, string whereClause, string orderByClause, string joinClause) where TModel : BaseModelMySQL, new()
        {
            // defaults
            if (tableName.IsBlank()) { tableName = TableName(typeof(TModel)); }
            if (idField.IsBlank()) { idField = TableIdField(typeof(TModel)); }

            var sql = "SELECT " + tableName + "." + idField + " FROM " + tableName;

            if (!joinClause.IsBlank())
            {
                sql += " " + joinClause;
            }

            if (!whereClause.IsBlank())
            {
                sql += " WHERE " + whereClause;
            }

            if (!orderByClause.IsBlank())
            {
                sql += " ORDER BY " + orderByClause;
            }

            return FetchList<TModel>(sql, idField);
        }

        public static List<TModel> FetchList<TModel>(string sql, string idField) where TModel : BaseModelMySQL, new()
        {
            // defaults
            List<TModel> list = new List<TModel>();
            using (MySqlConnection connection = new MySqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                using (MySqlCommand selectCommand = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader dataReader = selectCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            list.Add(Load<TModel>(dataReader[idField].ToString()));
                        }
                    }
                }
            }

            return list;
        }

        public static List<TModel> FetchPage<TModel>(Paginator paginator) where TModel : BaseModelMySQL, new()
        {
            return FetchPage<TModel>(paginator.CurrentPage, paginator.PageSize, null /*tableName*/, null /*idField*/, null /*joinClause*/, null /*orderByClause*/, null /*whereClause*/);
        }

        /*  public static List<TModel> FetchPage<TModel>(Paginator paginator, MySqlCommand command) where TModel : BaseModelMySQL, new()
          {
              if (paginator.CurrentPage < 0 || paginator.PageSize < 0)
              {
                  return null;
              }

              string query = command.CommandText.ToLower();
              string orderBy;
              string idField = TableIdField(typeof(TModel));
              string tableName = TableName(typeof(TModel));

              string commandBody = query.Substring(query.IndexOf("from"));

              if (query.Contains("order by"))
              {
                  orderBy = query.Substring(query.IndexOf("order by"));
                  commandBody = commandBody.Remove(commandBody.IndexOf("order by"));
              }
              else
              {
                  orderBy = "order by " + idField;
              }

              List<TModel> list = new List<TModel>();
              using (MySqlConnection connection = new MySqlConnection(ModelsCommon.FetchMysqlConnectionString()))
              {
                  connection.Open();

                  // WARNING: the third parameter idField must never contain the table name or appending it again will break the query.
                  string sql =
                      "SELECT " + idField + @" FROM (
                          SELECT row_number() OVER (" + orderBy + ") row, " + tableName + "." + idField + " " +
                          commandBody + @"
                        ) rowIndexs
                        WHERE row > @rowSetStart 
                        AND row <= @rowSetend";
                  using (MySqlCommand selectCommand = command.Clone())
                  {
                      selectCommand.CommandText = sql;
                      selectCommand.Connection = connection;
                      selectCommand.Parameters.AddWithValue("rowSetStart", ((paginator.CurrentPage - 1) * paginator.PageSize));
                      selectCommand.Parameters.AddWithValue("rowSetend", paginator.CurrentPage * paginator.PageSize);

                      using (MySqlDataReader dataReader = selectCommand.ExecuteReader())
                      {
                          while (dataReader.Read())
                          {
                              list.Add(Load<TModel>(dataReader[idField].ToString()));
                          }
                      }
                  }
              }

              return list;
          }
            2020-02-03
          */
        public static List<TModel> FetchPage<TModel>(Paginator paginator, object clauses) where TModel : BaseModelMySQL, new()
        {
            return FetchPage<TModel>(paginator.CurrentPage, paginator.PageSize, null /*tableName*/, null /*idField*/, clauses);
        }

        public static List<TModel> FetchPage<TModel>(Paginator paginator, string tableName, string idField, object clauses) where TModel : BaseModelMySQL, new()
        {
            return FetchPage<TModel>(paginator.CurrentPage, paginator.PageSize, tableName, idField, clauses);
        }

        /// <summary>Gives a paginated listing of models for List views, one page per request.  Order column is defaulted to "date_created".</summary>
        /// <typeparam name="TModel">Model class type; table/idField names assumed from TModel.</typeparam>
        /// <param name="pageNumber">Page number; starts at 1</param>
        /// <param name="pageSize">number of models per page</param>
        public static List<TModel> FetchPage<TModel>(int pageNumber, int pageSize) where TModel : BaseModelMySQL, new()
        {
            return FetchPage<TModel>(pageNumber, pageSize, null /*tableName*/, null /*idField*/, null /*orderByClause*/, null /*whereClause*/);
        }

        /// <summary>Gives a paginated listing of models for List views, one page per request.  Order column is defaulted to "date_created".</summary>
        /// <typeparam name="TModel">Model class type; table/idField names assumed from TModel.</typeparam>
        /// <param name="pageNumber">Page number; starts at 1</param>
        /// <param name="pageSize">number of models per page</param>
        /// <param name="clauses">object specifying keys: "order" and "where" to define strings for ORDER BY and WHERE clauses. i.e. new {order="key_id", where="fkey=7"}</param>
        public static List<TModel> FetchPage<TModel>(int pageNumber, int pageSize, object clauses) where TModel : BaseModelMySQL, new()
        {
            return FetchPage<TModel>(pageNumber, pageSize, null /*tableName*/, null /*idField*/, clauses);
        }

        /// <summary>Gives a paginated listing of models for List views, one page per request.  Order column is defaulted to "date_created".</summary>
        /// <typeparam name="TModel">Model class type</typeparam>
        /// <param name="pageNumber">Page number; starts at 1</param>
        /// <param name="pageSize">number of models per page</param>
        /// <param name="tableName">database table</param>
        /// <param name="idField">key for database table</param>
        public static List<TModel> FetchPage<TModel>(int pageNumber, int pageSize, string tableName, string idField) where TModel : BaseModelMySQL, new()
        {
            return FetchPage<TModel>(pageNumber, pageSize, tableName, idField, idField /* orderByClause */, null /* whereClause */);
        }

        /// <summary>Gives a paginated listing of models for List views, one page per request.</summary>
        /// <typeparam name="TModel">Model class type</typeparam>
        /// <param name="pageNumber">Page number; starts at 1</param>
        /// <param name="pageSize">number of models per page</param>
        /// <param name="tableName">database table</param>
        /// <param name="idField">key for database table</param>
        /// <param name="clauses">object specifying keys: "order" and "where" to define strings for ORDER BY and WHERE clauses. i.e. new {order="key_id", where="fkey=7"}</param>
        public static List<TModel> FetchPage<TModel>(int pageNumber, int pageSize, string tableName, string idField, object clauses) where TModel : BaseModelMySQL, new()
        {
            NameValueCollection dict = ObjectToDictionary(clauses);
            return FetchPage<TModel>(pageNumber, pageSize, tableName, idField, dict["join"], dict["order"], dict["where"]);
        }

        /// <summary>Gives a paginated listing of models for List views, one page per request.</summary>
        /// <typeparam name="TModel">Model class type</typeparam>
        /// <param name="pageNumber">Page number; starts at 1</param>
        /// <param name="pageSize">number of models per page</param>
        /// <param name="tableName">database table</param>
        /// <param name="idField">key for database table</param>
        /// <param name="orderByClause">order by clause inserted after ORDER BY in sql query.</param>
        /// <param name="whereClause">where clause inserted after WHERE in the sql query.</param>
        public static List<TModel> FetchPage<TModel>(int pageNumber, int pageSize, string tableName, string idField, string orderByClause, string whereClause) where TModel : BaseModelMySQL, new()
        {
            return FetchPage<TModel>(pageNumber, pageSize, tableName, idField, null/*joinClause*/, orderByClause, whereClause);
        }

        public static List<TModel> FetchPage<TModel>(int pageNumber, int pageSize, string tableName, string idField, string joinClause, string orderByClause, string whereClause) where TModel : BaseModelMySQL, new()
        {

            if (pageNumber < 0 || pageSize < 0)
            {
                return null;
            }

            if (tableName.IsBlank())
            {
                tableName = TableName(typeof(TModel));
            }

            if (idField.IsBlank())
            {
                idField = TableIdField(typeof(TModel));
            }

            if (String.IsNullOrEmpty(orderByClause))
            {
                orderByClause = tableName + "." + idField;
            }

            if (String.IsNullOrEmpty(joinClause))
            {
                joinClause = String.Empty;
            }
            if (!tableName.Contains("docit."))
                tableName = "docit." + tableName;
            string sqlWhere = String.IsNullOrEmpty(whereClause) ? String.Empty : "WHERE " + whereClause;

            List<TModel> list = new List<TModel>();
            using (MySqlConnection connection = new MySqlConnection(ModelsCommon.FetchMysqlConnectionString()))
            {
                connection.Open();

                // WARNING: the third parameter idField must never contain the table name or appending it again will break the query.
                string sql = String.Format(
                    @"SELECT {0} FROM (
                        SELECT row_number() OVER (ORDER BY {1}) row_num, {2}
                        FROM {3}
                        {4}
                        {5}
                      ) rowIndexs
                      WHERE row_num > @rowSetStart 
                      AND row_num <= @rowSetend", idField, orderByClause, tableName + "." + idField, tableName, joinClause, sqlWhere);
                //System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", sql);
                using (MySqlCommand selectCommand = new MySqlCommand(sql, connection))
                {
                    /*tableName
                    @"
                    SELECT " + idField + @" 
                    FROM (
                        SELECT row_number() OVER (ORDER BY " + orderByClause + ") row, " + idField + @"
                        FROM " + tableName + sqlWhere + @") rowIndexs
                    WHERE row > @rowSetStart 
                    AND row <= @rowSetend", connection)) {
                     */
                    selectCommand.Parameters.AddWithValue("rowSetStart", ((pageNumber - 1) * pageSize));
                    selectCommand.Parameters.AddWithValue("rowSetend", pageNumber * pageSize);

                    using (MySqlDataReader dataReader = selectCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            list.Add(Load<TModel>(dataReader[idField].ToString()));
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>Gives a count of the number of entries in a table.  For use with FetchPage to determine total number of pages.</summary>
        /// <param name="clauses">object containing where field; i.e. new { where = "myfield = 'myvalue'" }</param>
        public static int TotalCount<TModel>(object clauses)
        {
            var dict = ObjectToDictionary(clauses);
            return TotalCount(TableName(typeof(TModel)), dict["where"], dict["join"]);
        }

        /* public static int TotalCount<TModel>(MySqlCommand command)
         {
             return TotalCount(TableName(typeof(TModel)), command);
         }
           2020--02-03 */


        /*
        /// <summary>Gives a count of the number of entries in a table.</summary>
        public static int TotalCount<TModel>() {
            return TotalCount(TableName(typeof(TModel)));
        }
         */

        /*
        /// <summary>Gives a count of the number of entries in a table.  For use with FetchPage to determine total number of pages.</summary>
        /// <typeparam name="TModel">declare TModel to figure out table name</typeparam>
        /// <param name="whereClause">whereClause limiting the count</param>
        public static int TotalCount<TModel>(string whereClause) {
            return TotalCount(TableName(typeof(TModel)), whereClause);
        }
         */

        /// <summary>Gives a count of the number of entries in a table.  For use with FetchPage to determine total number of pages.</summary>
        /// <param name="tableName">tableName for TModel</param>
        public static int TotalCount(string tableName)
        {
            return TotalCount(tableName, (string)null /* whereClause */);
        }

        /// <summary>Gives a count of the number of entries in a table.  For use with FetchPage to determine total number of pages.</summary>
        /// <param name="tableName">tableName for TModel</param>
        /// <param name="whereClause">whereClause limiting the count</param>
        public static int TotalCount(string tableName, string whereClause)
        {
            return TotalCount(tableName, whereClause, null);
        }

        /* public static int TotalCount(string tableName, MySqlCommand command)
         {
             string sql = command.CommandText.ToLower();
             if (sql.Contains("order by"))
             {
                 // strip out the order by clause.
                 sql = sql.Remove(sql.IndexOf("order by"));
             }

             sql = "SELECT COUNT(*) FROM (Select 1 as x " + sql.Substring(sql.IndexOf("from")) + " ) rows";
             using (MySqlCommand countCommand = command.Clone())
             {
                 countCommand.CommandText = sql;
                 using (MySqlConnection connection = new MySqlConnection(ModelsCommon.FetchConnectionString()))
                 {
                     connection.Open();
                     countCommand.Connection = connection;
                     return (int)countCommand.ExecuteScalar();
                 }
             }
         }
           2020-02-03
              */

        public static int TotalCount(string tableName, string whereClause, string joinClause)
        {
            if (!tableName.Contains("docit."))
                tableName = "docit." + tableName;
            string sql = "SELECT COUNT(*) FROM " + tableName;
            int count = 0;

            if (!joinClause.IsBlank())
            {
                sql += " " + joinClause;
            }

            if (!whereClause.IsBlank())
            {
                sql += " WHERE " + whereClause;
            }
            // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", sql);
            using (MySqlConnection connection = new MySqlConnection(ModelsCommon.FetchMysqlConnectionString()))
            {
                connection.Open();

                using (MySqlCommand selectCommand = new MySqlCommand(sql, connection))
                {

                    selectCommand.CommandTimeout = 300;
                    try
                    { count = Convert.ToInt32(selectCommand.ExecuteScalar()); }
                    catch (Exception ex)
                    { System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", sql + " " + ex.Message.ToString()); }
                }
                connection.Close();
            }
            return count;
        }

        #endregion

        #region Audit Functions

        #region UpdateAuditLog
        protected virtual void UpdateAuditLog(string userId)
        {
            UpdateAuditLog(null, userId);
        }

        protected virtual void UpdateAuditLog(List<string> ignore, string userId)
        {
            // get changed fields
            List<string> changedFields = GetChangedFields();
            if (changedFields.Count == 0)
            {
                return;
            }

            // remove any fields that are to be ignored.
            if (ignore != null)
            {
                foreach (string field in ignore)
                {
                    changedFields.Remove(field);
                }
            }

            DateTime timeStamp = DateTime.Now;
            Dictionary<string, string> fieldData = GetTableMetaData();

            string query = @"INSERT INTO Audits (type, id, user_id, date_modified, field, original_value, new_value) 
                            VALUES (@type, @id, @user_id, @date_modified, @field, @original_value, @new_value)";
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                foreach (string field in changedFields)
                {
                    string originalValue = ConvertToAuditFormat(_originalData[field], fieldData[field], field);
                    string newValue = ConvertToAuditFormat(_data[field], fieldData[field], field);
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("type", this.GetType().Name.Replace("Section", ""));
                        command.Parameters.AddWithValue("id", id);
                        command.Parameters.AddWithValue("user_id", userId);
                        command.Parameters.AddWithValue("date_modified", timeStamp);
                        command.Parameters.AddWithValue("field", field);
                        command.Parameters.AddWithValue("original_value", originalValue);
                        command.Parameters.AddWithValue("new_value", newValue);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private string ConvertToAuditFormat(string value, string fieldType, string field)
        {
            if (value.IsBlank())
            {
                return "";
            }

            switch (fieldType)
            {
                case "datetime":
                    return value.ToDateTimeFormat();
                case "decimal":
                case "float":
                    return value.ToDecimalFormat();
                case "bit":
                    return value.ToHumanBool();
                case "int":
                    return ConvertIfId(value, field);
            }

            return value;
        }

        private string ConvertIfId(string value, string field)
        {
            if (field == "location_id")
            {
                Location location = Location.Load(value);
                if (location != null)
                {
                    return location.ToString();
                }
            }

            if (field == "item_id")
            {
                Item item = Item.Load(value);
                if (item != null)
                {
                    return item.ToString();
                }
            }

            if (field == "shipped_to" || field == "lab_id")
            {
                Lab lab = Lab.Load(value);
                if (lab != null)
                {
                    return lab.ToString();
                }
            }

            if (field.EndsWith("_unit"))
            {
                Unit unit = Unit.Load(value);
                if (unit != null)
                {
                    return unit.ToString();
                }
            }

            if (field.EndsWith("_by"))
            {
                User user = User.Load(value);
                if (user != null)
                {
                    return user.ToString();
                }
            }
            return value;
        }
        #endregion

        #region FetchAuditRecords
        public virtual Dictionary<DateTime, List<Audit>> FetchAuditRecords()
        {
            // take the name of the object as the type, if it contains Section remove it. aka PreparationSection becomes Preparation
            return FetchAuditRecords(this.GetType().Name.Replace("Section", ""));
        }

        public virtual Dictionary<DateTime, List<Audit>> FetchAuditRecords(string auditType)
        {
            return FetchAuditLog(auditType, id);
        }

        public virtual Dictionary<DateTime, List<Audit>> FetchAuditRecords(string auditType, string field)
        {
            return FetchAuditLog(auditType, null, field);
        }

        public static Dictionary<DateTime, List<Audit>> FetchAuditLog()
        {
            return FetchAuditLog(null, null, null);
        }

        public static Dictionary<DateTime, List<Audit>> FetchAuditLog(string auditType)
        {
            return FetchAuditLog(auditType, null, null);
        }

        public static Dictionary<DateTime, List<Audit>> FetchAuditLog(string auditType, string id)
        {
            return FetchAuditLog(auditType, id, null);
        }

        public static Dictionary<DateTime, List<Audit>> FetchAuditLog(string auditType, string id, string field)
        {
            string connectionString = ModelsCommon.FetchConnectionString();

            Dictionary<DateTime, List<Audit>> auditRecords = new Dictionary<DateTime, List<Audit>>();

            using (MySqlCommand command = new MySqlCommand("SELECT * FROM Audits WHERE  1 = 1 "))
            {
                if (!auditType.IsBlank())
                {
                    command.CommandText += " AND type = @type";
                    command.Parameters.AddWithValue("type", auditType);
                }

                if (!id.IsBlank())
                {
                    command.CommandText += " AND id = @id";
                    command.Parameters.AddWithValue("id", id);
                }

                if (!field.IsBlank())
                {
                    command.CommandText += " AND field = @field";
                    command.Parameters.AddWithValue("field", field);
                }

                command.CommandText += " ORDER BY date_modified DESC";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    command.Connection = connection;

                    using (MySqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            DateTime date_modified = (DateTime)dataReader["date_modified"];
                            if (!auditRecords.ContainsKey(date_modified))
                            {
                                auditRecords.Add(date_modified, new List<Audit>());
                            }

                            Audit record = BaseModel.Load<Audit>(dataReader["audit_id"].ToString());
                            auditRecords[date_modified].Add(record);
                        }
                    }
                }
            }

            return auditRecords;
        }
        #endregion

        #region Audit Comparison Functions
        protected bool AreEqualDouble(string field)
        {
            if (!_originalData[field].IsBlank() && !_data[field].IsBlank())
            {
                return double.Parse(_originalData[field]) == double.Parse(_data[field]);
            }

            return _originalData[field].IsBlank() && _data[field].IsBlank();
        }

        protected bool AreEqualDateTime(string field)
        {
            if (!_originalData[field].IsBlank() && !_data[field].IsBlank())
            {
                return DateTime.Parse(_originalData[field]) == DateTime.Parse(_data[field]);
            }

            return _originalData[field].IsBlank() && _data[field].IsBlank();
        }
        #endregion
        #endregion

        #region System.Object override methods

        public override string ToString()
        {
            if (IsNewRecord) { return "None"; }
            if (_data["name"].IsBlank()) { return "Unknown"; }
            return _data["name"];
        }

        public static bool operator ==(BaseModelMySQL x, BaseModelMySQL y)
        {
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return ReferenceEquals(x, null) && ReferenceEquals(y, null);
            }

            return x.id == y.id;
        }

        public static bool operator !=(BaseModelMySQL x, BaseModelMySQL y)
        {
            return !(x == y);
        }

        public override bool Equals(object obj)
        {
            BaseModelMySQL target = obj as BaseModelMySQL;
            return target != null && this == target;
        }

        public override int GetHashCode()
        { // defined for use in RemoveDuplicates<TModel>()
            return (_primaryKey + id).GetHashCode();
        }

        #endregion

        #region IComparable Members

        public virtual int CompareTo(object obj)
        { // defined for all cases of Array.Sort(model[])
            BaseModel target = obj as BaseModel;
            if (target != null)
            {
                return int.Parse(id).CompareTo(int.Parse(target.id));
            }

            throw new ArgumentException("object is not a BaseModel");
        }

        #endregion

        #region general static methods
        internal static string TableName(Type type)
        {
            return TypeName(type) + "s";
        }

        internal static string TableIdField(Type type)
        {
            return TypeName(type).ToUnderscore() + "_id";
        }

        internal static string TypeName(Type type)
        {
            var fullTypeName = type.ToString().Split('.');
            return fullTypeName[fullTypeName.Length - 1];
        }

        internal static NameValueCollection ObjectToDictionary(object param)
        {
            var keyObjDict = (IDictionary<string, object>)new System.Web.Routing.RouteValueDictionary(param); // TODO: when Microsoft releases source for System.Web.Routing.dll, see how Routing converts anonymous types to IDictionary
            var nameValue = new NameValueCollection();
            foreach (var item in keyObjDict)
            {
                if (item.Value != null)
                {
                    nameValue[item.Key] = item.Value.ToString();
                }
            }
            return nameValue;
        }

        public string GetModelName()
        {
            // return _tableName.TrimEnd('s');
            return TypeName(this.GetType());
        }

        public static TModel[] RemoveDuplicates<TModel>(TModel[] modelArray)
        { // http://stackoverflow.com/questions/9673/remove-duplicates-from-array
            var set = new HashSet<TModel>(modelArray); // creates hash using BaseModel.GetHashCode() as keys
            TModel[] result = new TModel[set.Count];
            set.CopyTo(result);
            return result;
        }

        #endregion
    }
}