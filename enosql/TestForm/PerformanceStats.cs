using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using V8.Net;

using enosql;

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
            public int id { get; set; }
            public string _id { get; set; }
        }

        public static string InsertPerfStat(StatSettings settings)
        {
            var db = new enosql.EnosqlDatabase(settings.dbpath, settings.writescheduleTime);
            if (!settings.withWarmUp)
            {
                db.Drop();
                db = new enosql.EnosqlDatabase(settings.dbpath, settings.writescheduleTime);
            }

            var Users = db.GetCollection<Users>();

            var t = new Users();
            t.FirstName = "Donny";
            t.LastName = "V.";
            t.id = 0;

            EnosqlResult ret = null;

            var StartTime = DateTime.Now;
            for (int i = 0, l = settings.count; i < l; i++)
            {
                t.id = (t.id + 1);
                ret = Users.Insert(t);
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
