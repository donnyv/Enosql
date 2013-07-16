using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace enosql.tests
{
    [TestClass]
    public class UnitTest1
    {
        public class User
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int id { get; set; }
            public string _id { get; set; }
        }

        public void CleanUp()
        {

        }

        [TestMethod]
        public void InsertPerfTest(int count)
        {
            var db = new enosql.EnosqlDatabase(@"c:\temp\test.jdb");
            var Users = db.GetCollection("Users");

            var t = new User();
            t.FirstName = "Donny";
            t.LastName = "V.";
            t.id = 0;

            EnosqlResult ret;

            var StartTime = DateTime.Now;
            for (int i = 0, l = count; i < l; i++)
            {
                t.id = (t.id + 1);
                ret = Users.Insert(t);
            }
            var EndTime = DateTime.Now;

            var TimeToComplete = EndTime.Subtract(StartTime);
            var result = "Total time to complete:" + TimeToComplete.ToString();

           
        }
    }
}
