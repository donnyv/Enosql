using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using V8.Net;

using enosql;
using enosql.Builders;

namespace TestForm
{
    public class PerformanceStats
    {
        public class StatSettings
        {
            public string dbpath { get; set; }
            public int count { get; set; }
            public double writescheduleTime = 600;
            public bool withWarmUp { get; set; }
        }

        public class Users
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int index { get; set; }
            public string _id { get; set; }
        }

        public static string InsertJSONPerfStat(StatSettings settings)
        {

            var db = new enosql.EnosqlDatabase(settings.dbpath, settings.writescheduleTime);
            if (!settings.withWarmUp)
            {
                db.Drop();
                db = new enosql.EnosqlDatabase(settings.dbpath, settings.writescheduleTime);
            }
            else
            {
                db.DropCollection("UsersJson");
            }

            var Users = db.GetCollection("UsersJson");


            EnosqlResult ret = null;

            string json = string.Empty;
            var StartTime = DateTime.Now;
            for (int i = 0, l = settings.count; i < l; i++)
            {
                json = @"{
		                    'FirstName' : 'Donny',
		                    'LastName' : 'V.',
		                    'index' : " + i +
	                    "}";
                ret = Users.Insert(json);
            }
            var EndTime = DateTime.Now;

            if (ret.IsError)
                throw new Exception(ret.Msg);

            var TimeToComplete = EndTime.Subtract(StartTime);
            return TimeToComplete.ToString() + " sec";
        }

        public static string InsertObjectPerfStat(StatSettings settings)
        {
            
            var db = new enosql.EnosqlDatabase(settings.dbpath, settings.writescheduleTime);
            if (!settings.withWarmUp)
            {
                db.Drop();
                db = new enosql.EnosqlDatabase(settings.dbpath, settings.writescheduleTime);
            }
            else
            {
                db.DropCollection<Users>();
            }

            var Users = db.GetCollection<Users>();

            var t = new Users();
            t.FirstName = "Donny";
            t.LastName = "V.";
            t.index = 0;

            EnosqlResult ret = null;

            var StartTime = DateTime.Now;
            for (int i = 0, l = settings.count; i < l; i++)
            {
                t.index = (t.index + 1);
                ret = Users.Insert(t);
            }
            var EndTime = DateTime.Now;

            if (ret.IsError)
                throw new Exception(ret.Msg);

            var TimeToComplete = EndTime.Subtract(StartTime);
            return TimeToComplete.ToString() + " sec";
        }

        public static string QueryPerStat1(StatSettings settings)
        {
            var db = new enosql.EnosqlDatabase(settings.dbpath, settings.writescheduleTime);
            var Users = db.GetCollection<Users>();

            EnosqlResult ret = null;

            var StartTime = DateTime.Now;
            var query = EnosqlQuery.EQ("FirstName", "Donny3");
            for (int i = 0, l = settings.count; i < l; i++)
            {
                ret = Users.Find(query);
            }
            var EndTime = DateTime.Now;

            if (ret.IsError)
                throw new Exception(ret.Msg);

            var TimeToComplete = EndTime.Subtract(StartTime);
            return TimeToComplete.ToString() + " sec";
        }

        public void CleanUp()
        {

        }
    }
}
