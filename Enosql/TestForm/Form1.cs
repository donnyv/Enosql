using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MongoDB.Bson;
using MongoDB.Driver;

//using V8.Net;
using Newtonsoft.Json;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //var m = new MongoClient();
            //var s = m.GetServer();
            //var d = s.GetDatabase("");

            //var n = new TestCol() { FirstName = "donny" };
            //d.GetCollection("").Insert(n);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            dynamic d = JsonConvert.DeserializeObject<dynamic>("{ first: 'donny' ");
            var check = d;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var StartTime = DateTime.Now;
            var db = new enosql.EnosqlDatabase(@"c:\temp\test.jdb");
            
            var collection1 = db.GetCollection("User1");
            var FlushTask1 = Task.Factory.StartNew(() => Insert1(collection1, "Donny", 10000));

            var collection2 = db.GetCollection("User2");
            var FlushTask2 = Task.Factory.StartNew(() => Insert1(collection2, "Raven", 10000));

            var EndTime = DateTime.Now;
            var TimeToComplete = EndTime.Subtract(StartTime);
            MessageBox.Show("Total time to complete:" + TimeToComplete.ToString());
            //var ret = collection.Insert(t);

            //t = new Person2();
            //t.FirstName = "Raven";
            //t.LastName = "V.";
            //collection.Insert(t);

            
        }

        public void Insert1(enosql.EnosqlCollection collection, string name, int count)
        {
            var t = new Person2();
            t.FirstName = name;
            t.LastName = "V.";
            t.id = 0;
            for (int i = 0; i < count; i++)
            {
                t.id = (t.id + 1);
                collection.Insert(t);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {

                var db = new enosql.EnosqlDatabase(@"c:\temp\test.jdb");
                var collection1 = db.GetCollection("User1");
                collection1.RemoveAll();
                Insert1(collection1, "Donny", 10);
                //var FlushTask1 = Task.Factory.StartNew(() => Insert1(collection1, "Donny", 10));
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var db = new enosql.EnosqlDatabase(@"c:\temp\test.jdb");
            var collection1 = db.GetCollection("User1");
            
            var ret = collection1.FindAll();
            MessageBox.Show(ret.Json);
        }

        //public class Persons : V8NativeObject
        //{
        //    public IEnumerable<Person> PersonCollection { get; set; }
        //}
        //public class Person : V8NativeObject
        //{
        //    public string FirstName { get; set; }
        //    public string LastName { get; set; }
        //}
        public class Person2
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int id { get; set; }
            public string _id { get; set; }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var db = new enosql.EnosqlDatabase(@"c:\temp\test.jdb");
            var collection1 = db.GetCollection("User1");

            var updatePerson = new Person2()
            {
                FirstName = "Raven",
		        LastName = "V.",
		        id = 1,
		        _id = "51c9031d5aa01704d8000002"
            };
            var ret = collection1.Insert<Person2>(updatePerson);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var db = new enosql.EnosqlDatabase(@"c:\temp\test.jdb");
            var collection1 = db.GetCollection("User1");
            var ret = collection1.Remove("51c9031d5aa01704d8000006");
            
        }

       
    }
}
