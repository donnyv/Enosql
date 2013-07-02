using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;
using System.Threading.Tasks;

using V8.Net;

using Newtonsoft.Json;

namespace enosql
{
    internal class EnosqlEngine
    {
        internal readonly V8Engine v8Engine = null;
        public string dbPath = string.Empty;
        public string NamespacePath
        {
            get
            {
                return dbPath + @"\system.namespaces.json";
            }
        }
        
        readonly Dictionary<string, string> _jslibs = new Dictionary<string, string>();
        readonly Queue<string> _dirtyQueue = new Queue<string>();
        readonly HashSet<string> _workPool = new HashSet<string>();
        readonly Timer _writeSchedule = new Timer(500);

        public EnosqlEngine(string dbasePath)
        {
            v8Engine = new V8Engine();
            dbPath = dbasePath;
            Start();
            _writeSchedule.Elapsed += _writeSchedule_Elapsed;
            _writeSchedule.Start();
        }

        void _writeSchedule_Elapsed(object sender, ElapsedEventArgs e)
        {

            if (_dirtyQueue.Count == 0)
                return;

            // This checks if the next collection is still being flushed.
            // If so then we dequeue it and then still it back in by enqueuing it.
            // Hopefully the next round it will be free to flush to file.
            if (_workPool.Contains(_dirtyQueue.Peek()))
            {
                lock (_dirtyQueue)
                {
                    _dirtyQueue.Enqueue(_dirtyQueue.Dequeue());
                }
                return;
            }

            // Get the next collection to be flushed and
            // move it to the work pool.
            string dirtyCollection = string.Empty;
            lock (_dirtyQueue)
            {
                dirtyCollection = _dirtyQueue.Dequeue();
                _workPool.Add(dirtyCollection);
            }

            // Create and start a parallel task
            Task FlushTask;
            if(dirtyCollection == "system.namespaces")
                FlushTask = Task.Factory.StartNew(() => FlushSystemNamespaces());
            else
                FlushTask = Task.Factory.StartNew(() => FlushCollection(dirtyCollection));
                
        }

        void FlushSystemNamespaces()
        {
            try
            {
                // get entire collection
                EnosqlResult ret = new EnosqlResult();
                v8Engine.WithContextScope = () =>
                {
                    Handle result = v8Engine.Execute("GetNamespaces();", "Enosql Console");
                    ret.IsError = result.IsError;
                    ret.Msg = result.IsError ? result.AsString : string.Empty;
                    ret.Json = result.IsError ? string.Empty : result.AsString;
                };

                if (ret.IsError)
                    throw new Exception(ret.Msg);

                // write to disk
                using (var file = new System.IO.FileStream(this.dbPath + "\\system.namespaces.json", FileMode.Truncate))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(ret.Json);
                    file.Write(info, 0, info.Length);
                }

                _workPool.Remove("system.namespaces");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void FlushCollection(string collectionName)
        {
            try
            {
                // get entire collection
                EnosqlResult ret = new EnosqlResult();
                v8Engine.WithContextScope = () =>
                {
                    Handle result = v8Engine.Execute("FindAll('" + collectionName + "');", "Enosql Console");
                    ret.IsError = result.IsError;
                    ret.Msg = result.IsError ? result.AsString : string.Empty;
                    ret.Json = result.IsError ? string.Empty : result.AsString;
                };

                if (ret.IsError)
                    throw new Exception(ret.Msg);

                // write to disk
                using (var file = new System.IO.FileStream(this.dbPath + "\\" + collectionName + ".json", FileMode.Truncate))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(ret.Json);
                    file.Write(info, 0, info.Length);
                }

                _workPool.Remove(collectionName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Start()
        {
            EnosqlResult ret = new EnosqlResult();
            v8Engine.WithContextScope = () =>
            {
                // Load javascript libraries
                Assembly dll = Assembly.GetExecutingAssembly();
                var scripts = new StringBuilder();
                scripts.Append("var dbe = {};");
                scripts.Append(new StreamReader(dll.GetManifestResourceStream("enosql.js.underscore-min.js")).ReadToEnd());
                scripts.Append(new StreamReader(dll.GetManifestResourceStream("enosql.js.ObjectId.js")).ReadToEnd());
                //scripts.Append(new StreamReader(dll.GetManifestResourceStream("enosql.js.util.js")).ReadToEnd());
                scripts.Append(new StreamReader(dll.GetManifestResourceStream("enosql.js.db.js")).ReadToEnd());
                //var s = scripts.ToString();
                Handle result = v8Engine.Execute(scripts.ToString(), "Enosql Console");
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
            };

            LoadNamespaces();
            LoadCollections();
        }

        public EnosqlResult LoadNamespaces()
        {
            if (!File.Exists(NamespacePath))
                return new EnosqlResult() { IsError = true, Msg = "Does not exist! => " + NamespacePath };

            EnosqlResult ret = new EnosqlResult();
            v8Engine.WithContextScope = () =>
            {
                string ns = File.ReadAllText(NamespacePath);
                string script = @"AddNamespaces(" + ns + ");";
                Handle result = v8Engine.Execute(script, "Enosql Console");
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
            };
            return ret;
        }

        class CollectionFile
        {
            public string id { get; set; }
            public dynamic data { get; set; }
        }
        public EnosqlResult LoadCollections()
        {
            if (!File.Exists(NamespacePath))
                return new EnosqlResult() { IsError = true, Msg = "Does not exist! => " + NamespacePath };

            string ns = File.ReadAllText(NamespacePath);
            dynamic namespaces = JsonConvert.DeserializeObject<dynamic>(ns);
            if (namespaces == null)
                return new EnosqlResult() { IsError = false, Msg = "Empty namespaces file" };

            var cols = new List<CollectionFile>();
            for (int i = 0, l = namespaces.Count; i < l; i++)
            {
                if (File.Exists(dbPath + @"\" + namespaces[i].fileName))
                {
                    var data = File.ReadAllText(dbPath + @"\" + namespaces[i].fileName);
                    cols.Add(new CollectionFile()
                    {
                        id = namespaces[i]._id,
                        data = JsonConvert.DeserializeObject<dynamic>(data)
                    });
                }
            }

            EnosqlResult ret = new EnosqlResult();
            v8Engine.WithContextScope = () =>
            {
                string script = @"InitialCollectionLoad(" + JsonConvert.SerializeObject(cols) + ");";
                Handle result = v8Engine.Execute(script, "Enosql Console");
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
            };
            return ret;
        }

        public void Dirty(string collectionName)
        {
            if (!_dirtyQueue.Contains(collectionName))
                _dirtyQueue.Enqueue(collectionName);
        }

    }
}
