using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

using V8.Net;

namespace enosql
{
    public class EnosqlDatabase
    {
        EnosqlEngine _engineRef = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbasePath"></param>
        /// <param name="writescheduletime">This adjusts the time interval in milliseconds of how often data is written to disk</param>
        public EnosqlDatabase(string dbasePath, double writescheduletime = 100)
        {
            if (dbasePath == null)
                throw new ArgumentNullException("dbasePath");


            if (!Directory.Exists(dbasePath))
            {
                CreateDatabase(dbasePath);
            }

            if (!OS.HasWritePermissionOnDir(dbasePath))
                throw new Exception("Permission denied!\n" + dbasePath);

            _engineRef = EnosqlEnginePool.GetInstance(dbasePath, writescheduletime);
        }

        public void CreateDatabase(string dbasePath)
        {
            dbasePath = dbasePath.ToLower();
            try
            {
                if (!dbasePath.EndsWith(".jdb"))
                    dbasePath = dbasePath + ".jdb";

                // Create system enosql files
                Directory.CreateDirectory(dbasePath);
                using (StreamWriter sw = File.CreateText(dbasePath + "\\system.namespaces.json"))
                {
                    sw.WriteLine("[]");
                }
                using (StreamWriter sw = File.CreateText(dbasePath + "\\system.metadata.json"))
                {
                    sw.WriteLine("{ version: 1.0.0 }");
                }
            }
            catch 
            {
                throw new Exception("Could not create database!");
            }
        }

        public void Drop()
        {
            EnosqlEnginePool.Remove(_engineRef.dbPath);
            Directory.Delete(_engineRef.dbPath, true);
        }

        public EnosqlResult CreateCollection(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            name = name.Trim().ToLower();
            EnosqlResult ret = new EnosqlResult();
            string errorMsg = string.Empty;
            if (!IsCollectionNameValid(name, out errorMsg))
            {
                ret.IsError = true;
                ret.Msg = errorMsg;
                return ret;
            }
            
            _engineRef.v8Engine.WithContextScope = () =>
            {
                var args = new InternalHandle[1];
                args[0] = _engineRef.v8Engine.CreateString(name);
                Handle result = _engineRef.v8Engine.GlobalObject.Call("CreateCollection", args);
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
            };

            if (ret.IsError)
                return ret;

            // create empty js file
            string ext = name.ToLower().EndsWith(".json") ? string.Empty : ".json";
            using (File.CreateText(_engineRef.dbPath + "\\" + name + ext)) { };

            // flush namespace
            _engineRef.Dirty("system.namespaces");

            return ret;
        }

        public EnosqlResult DropCollection(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            name = name.Trim();

            // get namespace info
            EnosqlResult retNameSpaceInfo = new EnosqlResult();
            _engineRef.v8Engine.WithContextScope = () =>
            {
                var args = new InternalHandle[1];
                args[0] = _engineRef.v8Engine.CreateString(name);
                Handle result = _engineRef.v8Engine.GlobalObject.Call("GetNamespace", args);
                retNameSpaceInfo.IsError = result.IsError;
                retNameSpaceInfo.Msg = result.IsError ? result.AsString : string.Empty;
                retNameSpaceInfo.Json = result.IsError ? string.Empty : result.AsString;
            };

            if (retNameSpaceInfo.IsError)
                return retNameSpaceInfo;

            // drop collection
            EnosqlResult retDropColl = new EnosqlResult();
            _engineRef.v8Engine.WithContextScope = () =>
            {
                var args = new InternalHandle[1];
                args[0] = _engineRef.v8Engine.CreateString(name);
                Handle result = _engineRef.v8Engine.GlobalObject.Call("DropCollection", args);
                retDropColl.IsError = result.IsError;
                retDropColl.Msg = result.IsError ? result.AsString : string.Empty;
            };

            // flush namespace
            _engineRef.Dirty("system.namespaces");

            //delete collection file
            var collFilePath = _engineRef.dbPath + "\\" + retNameSpaceInfo.DynamicJson.fileName;
            if(File.Exists(collFilePath))
                File.Delete(collFilePath);

            return retDropColl;
        }
        public EnosqlResult DropCollection<T>() where T : class
        {
            return DropCollection(typeof(T).Name);
        }

        public bool IsCollectionNameValid(string name, out string message)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (name == "")
            {
                message = "Collection name cannot be empty.";
                return false;
            }

            if (name.IndexOf('\0') != -1)
            {
                message = "Collection name cannot contain null characters.";
                return false;
            }

            if (Encoding.UTF8.GetBytes(name).Length > 121)
            {
                message = "Collection name cannot exceed 121 bytes (after encoding to UTF-8).";
                return false;
            }

            if (name.ToLower().StartsWith("system."))
            {
                message = "Collection name using reserved keywords.";
                return false;
            }

            if (name.IndexOf(" ") != -1)
            {
                message = "Collection name can not contain spaces.";
                return false;
            }

            // Get a list of invalid file characters. 
            string strTheseAreInvalidFileNameChars = new string(System.IO.Path.GetInvalidFileNameChars()); 
            Regex regFixFileName = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            if(regFixFileName.IsMatch(name))
            {
                message = "Collection name contains invalid characters.";
                return false;
            }

            message = null;
            return true;
        }

        public bool CollectionExists(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            name = name.Trim();
            bool isError = false;
            bool exists = false;
            _engineRef.v8Engine.WithContextScope = () =>
            {
                string script = @"CollectionExists('" + name + "');";
                Handle result = _engineRef.v8Engine.Execute(script, "Enosql Console");
                isError = result.IsError;
                exists = result.IsBoolean ? result.AsBoolean : false;
            };
            return isError ? false : exists;
        }

        public EnosqlCollection GetCollection(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (!CollectionExists(name))
            {
                var ret = CreateCollection(name);
                if (ret.IsError)
                {
                    throw new Exception(ret.Msg);
                }
            }

            return new EnosqlCollection(_engineRef, this, name);
        }
        public EnosqlCollection<T> GetCollection<T>() where T : class
        {
            var name = typeof(T).Name;
            if (!CollectionExists(name))
            {
                var ret = CreateCollection(name);
                if (ret.IsError)
                {
                    throw new Exception(ret.Msg);
                }
            }

            return new EnosqlCollection<T>(_engineRef, this, name);
        }
    }
}
