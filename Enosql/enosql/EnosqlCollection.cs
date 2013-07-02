using System;
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
        string _collectionName = string.Empty;

        internal EnosqlCollection(EnosqlEngine engineRef, string name)
        {
            _engineRef = engineRef;
            _collectionName = name;
        }

        public EnosqlResult Insert<T>(T document)
        {
            EnosqlResult ret = new EnosqlResult();
            _engineRef.v8Engine.WithContextScope = () =>
            {
                string script = @"Insert('" + _collectionName + "'," + JsonConvert.SerializeObject(document) + ");";
                Handle result = _engineRef.v8Engine.Execute(script, "Enosql Console");
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
            };

            if (!ret.IsError)
                _engineRef.Dirty(_collectionName);

            return ret;
        }

        public EnosqlResult Insert(string json)
        {
            EnosqlResult ret = new EnosqlResult();
            _engineRef.v8Engine.WithContextScope = () =>
            {
                string script = @"Insert('" + _collectionName + "'," + json + ");";
                Handle result = _engineRef.v8Engine.Execute(script, "Enosql Console");
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
            };

            if (!ret.IsError)
                _engineRef.Dirty(_collectionName);

            return ret;
        }

        public EnosqlResult FindById(string _id)
        {
            var ret = new EnosqlResult();
            _engineRef.v8Engine.WithContextScope = () =>
            {
                string script = @"FindById('" + _collectionName + "','" + _id + "');";
                Handle result = _engineRef.v8Engine.Execute(script, "Enosql Console");
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
                ret.Json = result.IsError ? string.Empty : result.AsString;
            };

            return ret;
        }

        public EnosqlResult FindAll()
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

        public EnosqlResult Update<T>(T document)
        {
            EnosqlResult ret = new EnosqlResult();
            _engineRef.v8Engine.WithContextScope = () =>
            {
                string script = @"Update('" + _collectionName + "'," + JsonConvert.SerializeObject(document) + ");";
                Handle result = _engineRef.v8Engine.Execute(script, "Enosql Console");
                ret.IsError = result.IsError;
                ret.Msg = result.IsError ? result.AsString : string.Empty;
            };

            if (!ret.IsError && ret.DynamicJson != -1)
                _engineRef.Dirty(_collectionName);

            return ret;
        }

    }

    public class EnosqlCollection<T> : EnosqlCollection
    {
        internal EnosqlCollection(EnosqlEngine engineRef, string name)
            : base(engineRef, name){}

        public EnosqlResult<T> FindById<T>(string _id)
        {
            return new EnosqlResult<T>(base.FindById(_id));
        }
        new public EnosqlResult<T> FindAll()
        {
            return new EnosqlResult<T>(base.FindAll());
        }
    }
}
