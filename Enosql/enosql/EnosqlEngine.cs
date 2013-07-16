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
        readonly Timer _writeSchedule = null;

        public EnosqlEngine(string dbasePath, double writescheduletime = 600)
        {
            v8Engine = new V8Engine();
            dbPath = dbasePath;
            _writeSchedule = new Timer(writescheduletime);
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
            if (dirtyCollection == "system.namespaces")
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
                    var args = new InternalHandle[1];
                    args[0] = v8Engine.CreateString(collectionName);
                    Handle result = v8Engine.GlobalObject.Call("FindAll", args);
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
                Handle result = v8Engine.Execute(scripts.ToString(), "Enosql Console");
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
            };

            LoadNamespaces();
            if (!LoadCollections().IsError)
                BuildCollectionIds();
        }

        public void Stop()
        {
            _writeSchedule.Stop();
        }

        public EnosqlResult LoadNamespaces()
        {
            if (!File.Exists(NamespacePath))
                return new EnosqlResult() { IsError = true, Msg = "Does not exist! => " + NamespacePath };

            string ns = File.ReadAllText(NamespacePath);
            if (string.IsNullOrWhiteSpace(ns))
                return new EnosqlResult() { IsError = true, Msg = "Empty! => " + NamespacePath };

            // create any missing collections
            CreateMissingCollectionFiles(ns);

            EnosqlResult ret = new EnosqlResult();
            v8Engine.WithContextScope = () =>
            {
                var args = new InternalHandle[1];
                args[0] = v8Engine.CreateString(ns);
                Handle result = v8Engine.GlobalObject.Call("AddNamespaces", args);
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
            };
            return ret;
        }

        void CreateMissingCollectionFiles(string NS)
        {
            var ns = JsonConvert.DeserializeObject<List<dynamic>>(NS);
            string filename = string.Empty;
            string ext = string.Empty;
            string fullpath = string.Empty;
            for (int i = 0, l = ns.Count; i < l; i++)
            {
                // create empty js file
                filename = ns[i].fileName;
                ext = filename.ToLower().EndsWith(".json") ? string.Empty : ".json";
                fullpath = dbPath + "\\" + filename + ext;
                if (!File.Exists(fullpath))
                {
                    using (File.CreateText(fullpath)) { };
                }
            }
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
                    if (string.IsNullOrWhiteSpace(data))
                        data = "[]";

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
                var args = new InternalHandle[1];
                args[0] = v8Engine.CreateString(JsonConvert.SerializeObject(cols));
                Handle result = v8Engine.GlobalObject.Call("InitialCollectionLoad", args);
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
            };
            return ret;
        }

        public EnosqlResult BuildCollectionIds()
        {
            EnosqlResult ret = new EnosqlResult();
            v8Engine.WithContextScope = () =>
            {
                Handle result = v8Engine.GlobalObject.Call("BuildCollectionIds");
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
