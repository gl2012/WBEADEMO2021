/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace WBEADMS.Models
{
    public class Status : BaseModel
    {
        #region class variables
        private static readonly List<Status> _status_list;
        private static readonly Dictionary<string, Status> _reverse_lookup;

        #endregion

        #region private members
        private const string _table_name = "Statuses";
        private const string _id_field = "status_id";

        private static readonly string[] _table_fields = new string[] {
            "status_id",
            "sort",
            "name",
            "section",
            "on_save",
            "on_commit",
            "form_view",
            "details_view",
            "is_completed"
        };
        #endregion

        #region contructors
        static Status()
        {
            _status_list = FetchAll<Status>(_table_name, _id_field, null);
            _reverse_lookup = FetchReverseLookUp();
        }

        public Status() : base(_table_name, _id_field, _table_fields) { }
        #endregion

        #region properties
        public static Status Opened
        {
            get
            {
                // NOTE: 1 is opened stated.
                return Status.Load("1");
            }
        }

        public Status OnSave
        {
            get
            {
                return Load(_data["on_save"]);
            }
        }

        public Status OnCommit
        {
            get
            {
                return Load(_data["on_commit"]);
            }
        }

        public int Sort
        {
            get
            {
                return int.Parse(_data["sort"]);
            }
            set
            {
                _data["flow"] = value.ToString();
            }
        }

        public string name
        {
            get
            {
                return _data["name"];
            }
            set
            {
                _data["name"] = value;
            }
        }

        public string SectionName
        {
            get
            {
                return _data["section"];
            }
            set
            {
                _data["section"] = value;
            }
        }

        public bool IsLastStatusBeforeCommitted
        {
            get
            {
                return _data["on_save"].IsBlank();
            }
        }

        public bool IsComplete
        {
            get
            {
                return _data["is_completed"] == "True";
            }
        }

        public string form_view
        {
            get
            {
                return _data["form_view"];
            }
        }

        public string details_view
        {
            get
            {
                return _data["details_view"];
            }
        }

        protected string is_completed
        {
            get
            {
                return _data["is_completed"];
            }
        }
        #endregion

        #region Static Methods
        public static Status Load(string status_id)
        {
            foreach (Status status in _status_list)
            {
                if (status.id == status_id)
                {
                    return status;
                }
            }
            return null;
        }

        public static List<Status> FetchAll()
        {
            return _status_list;
        }

        public static SelectList FetchSelectList(object status_id)
        {
            return new SelectList(_status_list, "id", "name", status_id);
        }

        private static Dictionary<string, Status> FetchReverseLookUp()
        {
            Dictionary<string, Status> reverseFlowLookUp = new Dictionary<string, Status>();

            string sql = @"SELECT status_id AS current_state, previous_state FROM Statuses WHERE previous_state IS NOT NULL";
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                using (SqlCommand selectCommand = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string state = reader["current_state"].ToString();
                            Status previousState = Status.Load(reader["previous_state"].ToString());

                            //we don't allow there to be duplicate states in the flow.
                            if (reverseFlowLookUp.ContainsKey(state))
                            {
                                throw new Exception("ERROR: Broken Flow duplicate state levels detected. Contact your administrator.");
                            }

                            reverseFlowLookUp.Add(state, previousState);
                        }
                    }
                }
            }

            return reverseFlowLookUp;
        }

        #endregion

        public override void Validate()
        {
            throw new NotSupportedException();
        }

        public override void Save()
        {
            throw new NotSupportedException();
        }

        public Status NextState(string sectionName, bool isCommit)
        {
            if (sectionName == SectionName)
            {
                if (isCommit)
                {
                    return Status.Load(_data["on_commit"]);
                }
                else
                {
                    if (!_data["on_save"].IsBlank())
                    {
                        return Status.Load(_data["on_save"]);
                    }
                }
            }
            //if a different section was submitted or it was a save that had a null on_save, no change.
            return this;
        }

        public Status PreviousState()
        {
            if (_reverse_lookup.ContainsKey(id))
            {
                return _reverse_lookup[id];
            }

            return null;
        }
    }
}