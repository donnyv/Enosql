using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver;

using V8.Net;
using Newtonsoft.Json;

using enosql;
using enosql.Builders;
using enosql.JSON;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            

            //var db = new enosql.EnosqlDatabase(@"c:\temp\test.jdb");
            //var collection1 = db.GetCollection<User>();

            //var updatePerson = new User()
            //{
            //    FirstName = "Raven",
            //    LastName = "V.",
            //    id = 1,
            //    _id = "51c9031d5aa01704d8000002"
            //};
            //var ret = collection1.Insert(updatePerson);

            //var ret2 = collection1.Insert<Tasks>(new Tasks()
            //{
            //    _id = "51c9031d5aa01704d8000003",
            //    task = "get stuff done"
            //});


            //var p = collection1.FindById("51c9031d5aa01704d8000002");
            //var d1 = p.Data;

            //var t = collection1.FindById<Tasks>("51c9031d5aa01704d8000003");
            //var d2 = t.Data;

            //var task = t.Data[0];

            //var Pdata = p.Data[0];
            //Pdata.LastName = "Velazquez";
            //var ret3 = collection1.Save(Pdata);
        }

        private void btnInsertJSONPerf_Click(object sender, EventArgs e)
        {
            InsertObjectPerf();
        }

        private void btnInsertObjectPerf_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            InsertObjectPerf();
            this.Cursor = Cursors.Default;
        }

        private void btnQueryPerfTest_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            QueryPerf();
            this.Cursor = Cursors.Default;
        }

        void InsertJSONPerf()
        {
            var settings = new PerformanceStats.StatSettings();
            settings.dbpath = @"c:\temp\enosqlTest.jdb";
            settings.writescheduleTime = int.Parse(txtWriteTime.Text);

            var results = new StringBuilder();
            var counts = txtCount.Text.Split(",".ToCharArray());
            var runtimes = int.Parse(txtRunTimes.Text);
            string stat = string.Empty;
            string header = "insert {0} records, 100k each - JSON Strings - write scheduled every {1} msec" + Environment.NewLine;
            for (int i = 0, l = counts.Length; i < l; i++)
            {
                settings.count = int.Parse(counts[i]);

                results.Append(string.Format(header, settings.count, settings.writescheduleTime));

                settings.withWarmUp = false;
                results.Append("no warm up" + Environment.NewLine);
                results.Append("---------------------------------------------" + Environment.NewLine);
                for (int j = 0; j < runtimes; j++)
                {
                    results.Append(PerformanceStats.InsertObjectPerfStat(settings) + Environment.NewLine);
                    Thread.Sleep(500);
                }
                results.Append(Environment.NewLine);

                settings.withWarmUp = true;
                results.Append("with warm up" + Environment.NewLine);
                results.Append("---------------------------------------------" + Environment.NewLine);
                for (int j = 0; j < runtimes; j++)
                {
                    results.Append(PerformanceStats.InsertObjectPerfStat(settings) + Environment.NewLine);
                    Thread.Sleep(500);
                }
            }

            txtResult.Text = results.ToString();
        }

        void InsertObjectPerf()
        {
            var settings = new PerformanceStats.StatSettings();
            settings.dbpath = @"c:\temp\enosqlTest.jdb";
            settings.writescheduleTime = int.Parse(txtWriteTime.Text);

            var results = new StringBuilder();
            var counts = txtCount.Text.Split(",".ToCharArray());
            var runtimes = int.Parse(txtRunTimes.Text);
            string stat = string.Empty;
            string header = "insert {0} records, 100k each - Serialize Object - write scheduled every {1} msec" + Environment.NewLine;
            for (int i = 0, l = counts.Length; i < l; i++)
            {
                settings.count = int.Parse(counts[i]);

                results.Append(string.Format(header, settings.count, settings.writescheduleTime));

                settings.withWarmUp = false;
                results.Append("no warm up" + Environment.NewLine);
                results.Append("---------------------------------------------" + Environment.NewLine);
                for (int j = 0; j < runtimes; j++)
                {
                    results.Append(PerformanceStats.InsertObjectPerfStat(settings) + Environment.NewLine);
                    Thread.Sleep(500);
                }
                results.Append(Environment.NewLine);

                settings.withWarmUp = true;
                results.Append("with warm up" + Environment.NewLine);
                results.Append("---------------------------------------------" + Environment.NewLine);
                for (int j = 0; j < runtimes; j++)
                {
                    results.Append(PerformanceStats.InsertObjectPerfStat(settings) + Environment.NewLine);
                    Thread.Sleep(500);
                }
            }

            txtResult.Text = results.ToString();
        }

        void QueryPerf()
        {
            var settings = new PerformanceStats.StatSettings();
            settings.dbpath = @"c:\temp\enosqlTest.jdb";
            settings.writescheduleTime = int.Parse(txtWriteTime.Text);

            var results = new StringBuilder();
            var counts = txtCount.Text.Split(",".ToCharArray());
            var runtimes = int.Parse(txtRunTimes.Text);
            string stat = string.Empty;
            string header = "query {0} records, 100k each" + Environment.NewLine;
            for (int i = 0, l = counts.Length; i < l; i++)
            {
                settings.count = int.Parse(counts[i]);

                results.Append(string.Format(header, settings.count, settings.writescheduleTime));

                settings.withWarmUp = false;
                results.Append("no warm up" + Environment.NewLine);
                results.Append("---------------------------------------------" + Environment.NewLine);
                for (int j = 0; j < runtimes; j++)
                {
                    results.Append(PerformanceStats.QueryPerStat1(settings) + Environment.NewLine);
                    Thread.Sleep(500);
                }
                results.Append(Environment.NewLine);

                settings.withWarmUp = true;
                results.Append("with warm up" + Environment.NewLine);
                results.Append("---------------------------------------------" + Environment.NewLine);
                for (int j = 0; j < runtimes; j++)
                {
                    results.Append(PerformanceStats.QueryPerStat1(settings) + Environment.NewLine);
                    Thread.Sleep(500);
                }
            }

            txtResult.Text = results.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var db = new enosql.EnosqlDatabase(@"c:\temp\enosqlTest.jdb");
            var collection1 = db.GetCollection<PerformanceStats.Users>();

            //collection1.Insert(new PerformanceStats.Users()
            //{
            //   FirstName = "Raven",
            //   LastName = "Velazquez"
            //});
            //collection1.Insert(new PerformanceStats.Users()
            //{
            //    FirstName = "Donny",
            //    LastName = "Velazquez"
            //});
            //collection1.Insert(new PerformanceStats.Users()
            //{
            //    FirstName = "Jane",
            //    LastName = "Velazquez"
            //});

            //var ret = collection1.Find(EnosqlQuery.EQ("FirstName", "Jane"));
            
            var strArray = new string[] { "jane", "raven", "donny" };
            var intArray = new int[] { 1, 2, 3, 4 };


            var query = EnosqlQuery.EQ("index", 2);
            var query1 = EnosqlQuery.EQ("FirstName", "Donny1");
            var query2 = EnosqlQuery.EQ("FirstName", "donny2");
            var query3 = EnosqlQuery.EQi("FirstName", "donny2");
            var query4 = EnosqlQuery.EQ("FirstName", "Donny2");

            var AndQuery1 = EnosqlQuery.And(query, query3);

            var OrQuery = EnosqlQuery.Or("FirstName", new JSONValue("Donny1"), new JSONValue("Donny2"));

            var result = collection1.Find(OrQuery);

            this.Cursor = Cursors.Default;
        }

        List<PerformanceStats.Users> Find()
        {
            //var db = new MongoDB.Driver.MongoDatabase(new MongoDB.Driver.MongoServer(new MongoDB.Driver.MongoServerSettings()));
            //var col = db.GetCollection("");
            //Query.EQ("", "");

           

            return new List<PerformanceStats.Users>();
        }

        
    }
}
