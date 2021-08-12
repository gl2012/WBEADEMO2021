using System.Collections.Generic;

namespace WBEADMS.Models
{
    public class ECOCData_View : BaseModel
    {
        #region private members and properties
        public string SITE { get { return _data["SITE"]; } set { _data["SITE"] = value; } }
        public string DATE { get { return _data["DATE"]; } set { _data["DATE"] = value; } }
        //public string note_attachment_id{ get {return _data["note_attachment_id"]; } set { _data["note_attachment_id"] = value; } }
        public string Size { get { return _data["Size"]; } set { _data["Size"] = value; } }
        public string TVOC { get { return _data["TVOC"]; } set { _data["TVOC"] = value; } }
        public string TVOU { get { return _data["TVOU"]; } set { _data["TVOU"] = value; } }
        public string WEBA_ID { get { return _data["WEBA_ID"]; } set { _data["WEBA_ID"] = value; } }
        public string TID { get { return _data["TID"]; } set { _data["TID"] = value; } }
        public string WEBA_NOTE { get { return _data["WBEA NOTES"]; } set { _data["WBEA NOTES"] = value; } }
        public string ID { get { return _data["ECOCData_View_Id"]; } set { _data["ECOCData_View_Id"] = value; } }
        public string DateActaul { get { return _data["Date_Actaul"]; } set { _data["Date_Actaul"] = value; } }

        #endregion
        public ECOCData_View() : base() { }
        public override void Validate()
        {
            return;
        }
        public static List<ECOCData_View> FetchAll(string whereClause)
        {
            return FetchAll<ECOCData_View>(new { where = whereClause, order = "SITE DESC" });
        }
    }
}
