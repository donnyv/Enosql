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

//using MongoDB.Bson;
//using MongoDB.Driver;

using V8.Net;
using Newtonsoft.Json;

using enosql;

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

        private void btnInsertPerf_Click(object sender, EventArgs e)
        {
            //var db = new enosql.EnosqlDatabase(@"c:\temp\test.jdb");
            //var collection1 = db.GetCollection<PerformanceStats.Users>();

            //var Person = new PerformanceStats.Users()
            //{
            //    FirstName = "Raven",
            //    LastName = "V.",
            //    id = 1,
            //    _id = "51c9031d5aa01704d8000002"
            //};
            //var ret = collection1.Insert(Person);
            //File.Delete(@"C:\temp\enosqltest.jdb");

            //InsertJSONPerf();

            InsertObjectPerf();
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


        private void button3_Click(object sender, EventArgs e)
        {
            var db = new enosql.EnosqlDatabase(@"c:\temp\test.jdb");
            var collection1 = db.GetCollection<PerformanceStats.Users>();

            //var Person = new PerformanceStats.Users()
            //{
            //    FirstName = "Raven",
            //    LastName = "V.",
            //    id = 1,
            //    _id = "51c9031d5aa01704d8000002"
            //};
            //var ret = collection1.Insert(Person);

            db.DropCollection<PerformanceStats.Users>();
        }
    }
}
