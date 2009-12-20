using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using AwManaged.Configuration.Interfaces;

//using Aw.Data;

namespace Aw.Common.Config
{
    [Serializable]
    public abstract class BaseConfiguration<T> : IConfiguration where T : IConfiguration, new()
    {
        public static T GetConfiguration(string name)
        {
            throw new NotImplementedException();

            //var config = new T { Db = new AwDataContext(), Record = new core_configuration() };
            //var record = from p in config.Db.core_configurations where p.configuration_type == config.GetType().ToString() select p;
            //if (record.Count() == 0)
            //{
            //    config.Record.configuration_type = config.GetType().ToString();
            //    config.Record.configuration_id = Guid.NewGuid();
            //    config.Record.configuration_name = "New config";
            //    config.Record.confgiuration_image = new Binary(Serialization.Serialize(config));
            //    config.Db.core_configurations.InsertOnSubmit(config.Record);
            //    config.Db.SubmitChanges();
            //}
            //else
            //{
            //    config.Record = record.Single();
            //}
            //return config;
        }

        /// <summary>
        /// Saves this configuration.
        /// </summary>
        public void Save()
        {
            throw new NotImplementedException();
            //Db.SubmitChanges();
        }


        #region IIdentifiable Members

        [Description("Displayed name of the object for user interface purposes.")]
        [Category("Identification")]
        [ReadOnly(true)]
        public string DisplayName { get; set; }
        [Description("Globally unique identification of the object.")]
        [Category("Identifiaction")]
        [ReadOnly(true)]
        public Guid Id { get; set; }

        #endregion

        #region IConfiguration Members
        //[NonSerialized]
        //private AwDataContext _db;
        //[NonSerialized]
        //private core_configuration _record;

        //public AwDataContext Db { get{ return _db;} set{ _db = value;} }
        //public core_configuration Record { get { return _record; } set { _record = value; } }
        #endregion
    }
}
