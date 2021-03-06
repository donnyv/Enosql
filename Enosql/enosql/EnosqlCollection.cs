﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.CSharp;

using V8.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
//using MongoDB.Bson;

namespace enosql
{
    public class EnosqlCollection
    {
        EnosqlEngine _engineRef = null;
        EnosqlDatabase _db = null;
        string _collectionName = string.Empty;

        internal EnosqlCollection(EnosqlEngine engineRef, EnosqlDatabase db, string name)
        {
            _engineRef = engineRef;
            _db = db;
            _collectionName = name.ToLower();
        }

        public EnosqlResult Insert(string json)
        {
            EnosqlResult ret = new EnosqlResult();
            _engineRef.v8Engine.WithContextScope = () =>
            {
                var args = new InternalHandle[2];
                args[0] = _engineRef.v8Engine.CreateString(_collectionName);
                args[1] = _engineRef.v8Engine.CreateString(json);
                Handle result = _engineRef.v8Engine.GlobalObject.Call("Insert", args);
                ret.IsError = result.IsError;
                ret.Msg = result.AsString;
            };

            if (!ret.IsError)
                _engineRef.Dirty(_collectionName);

            return ret;
        }
        public EnosqlResult Insert<T>(T document)
        {
            return this.Insert(JsonConvert.SerializeObject(document));
        }
        
        public virtual EnosqlResult Find(string query)
        {
            var ret = new EnosqlResult();
            _engineRef.v8Engine.WithContextScope = () =>
            {
                var args = new InternalHandle[2];
                args[0] = _engineRef.v8Engine.CreateString(_collectionName);
                args[1] = _engineRef.v8Engine.CreateString(query);
                Handle result = _engineRef.v8Engine.GlobalObject.Call("Find", args);
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
                ret.Json = result.IsError ? string.Empty : result.AsString;
            };

            return ret;
        }
        public EnosqlResult<T> Find<T>(string query)
        {
            return new EnosqlResult<T>(Find(query));
        }

        public virtual EnosqlResult FindById(string _id)
        {
            var ret = new EnosqlResult();
            _engineRef.v8Engine.WithContextScope = () =>
            {
                var args = new InternalHandle[2];
                args[0] = _engineRef.v8Engine.CreateString(_collectionName);
                args[1] = _engineRef.v8Engine.CreateString(_id);
                Handle result = _engineRef.v8Engine.GlobalObject.Call("FindById", args);
                ret.IsError = result.IsError;
                ret.Msg =  result.IsError ? result.AsString : string.Empty;
                ret.Json = result.IsError ? string.Empty : result.AsString;
            };

            return ret;
        }
        public EnosqlResult<T> FindById<T>(string _id)
        {
            return new EnosqlResult<T>(FindById(_id));
        }

        public virtual EnosqlResult FindAll()
        {
            EnosqlResult ret = new EnosqlResult();
            _engineRef.v8Engine.WithContextScope = () =>
            {
                string script = @"FindAll('" + _collectionName + "');";
                Handle result = _engineRef.v8Engine.Execute(script, "Enosql Console");
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
                ret.Json = result.IsError ? string.Empty : result.AsString;
            };
            return ret;
        }
        public EnosqlResult<T> FindAll<T>()
        {
            return new EnosqlResult<T>(FindAll());
        }

        public EnosqlResult Remove(string _id)
        {
            EnosqlResult ret = new EnosqlResult();
            _engineRef.v8Engine.WithContextScope = () =>
            {
                string script = @"Remove('" + _collectionName + "', '" + _id + "');";
                Handle result = _engineRef.v8Engine.Execute(script, "Enosql Console");
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
                ret.Json = result.IsError ? string.Empty : result.AsString;
            };

            if (!ret.IsError && ret.DynamicJson != -1)
                _engineRef.Dirty(_collectionName);

            return ret;
        }

        public EnosqlResult RemoveAll()
        {
            EnosqlResult ret = new EnosqlResult();
            _engineRef.v8Engine.WithContextScope = () =>
            {
                string script = @"RemoveAll('" + _collectionName + "');";
                Handle result = _engineRef.v8Engine.Execute(script, "Enosql Console");
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
                ret.Json = result.IsError ? string.Empty : result.AsString;
            };

            if (!ret.IsError)
                _engineRef.Dirty(_collectionName);

            return ret;
        }

        public EnosqlResult Save(string json)
        {
            EnosqlResult ret = new EnosqlResult();
            _engineRef.v8Engine.WithContextScope = () =>
            {
                string script = @"Save('" + _collectionName + "'," + json + ");";
                Handle result = _engineRef.v8Engine.Execute(script, "Enosql Console");
                ret.IsError = result.IsError;
                ret.Msg = result.AsString;
            };

            if (!ret.IsError && ret.DynamicJson != -1)
                _engineRef.Dirty(_collectionName);

            return ret;
        }
        public EnosqlResult Save<T>(T document)
        {
            return Save(JsonConvert.SerializeObject(document));
        }

        public EnosqlResult Drop()
        {
            return _db.DropCollection(_collectionName);
        }
    }

    public class EnosqlCollection<T> : EnosqlCollection
    {
        internal EnosqlCollection(EnosqlEngine engineRef, EnosqlDatabase db, string name)
            : base(engineRef, db, name) { }

        public new EnosqlResult<T> Find(string query)
        {
            return new EnosqlResult<T>(base.Find(query));
        }

        public new EnosqlResult<T> FindById(string _id)
        {
            return new EnosqlResult<T>(base.FindById(_id));
        }

        public new EnosqlResult<T> FindAll()
        {
            return new EnosqlResult<T>(base.FindAll());
        }
    }
}
