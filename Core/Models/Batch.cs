/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */

namespace WBEADMS.Models
{
    public class Batch : BaseModel
    {

        public Batch() : base() { }

        #region properties
        public string batch_id
        {
            get
            {
                return _data["batch_id"];
            }

            set
            {
                _data["batch_id"] = value;
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

        public SampleResult[] SampleResults
        {
            get
            {
                return LoadByForeignKey<SampleResult>();
            }
        }
        #endregion

        public static Batch Load(string id)
        {
            return Load<Batch>(id);
        }

        public static Batch FetchByName(string name)
        {
            return Fetch<Batch>("name", name);
        }

        public override void Validate()
        {
            using (ModelException errors = new ModelException())
            {
                errors.AddError(name.CheckRequired("name"));
            }
        }
    }
}