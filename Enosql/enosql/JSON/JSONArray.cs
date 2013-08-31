using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace enosql.JSON
{
    public class JSONArray
    {
        string _value = null;

        public JSONArray(int[] value)
        {
            _value = JsonConvert.SerializeObject(value);
        }
        public static implicit operator JSONArray(int[] value)
        {
            return new JSONArray(value);
        }

        public JSONArray(double[] value)
        {
           _value = JsonConvert.SerializeObject(value);
        }
        public static implicit operator JSONArray(double[] value)
        {
            return new JSONArray(value);
        }

        public JSONArray(bool[] value)
        {
            _value = JsonConvert.SerializeObject(value);
        }
        public static implicit operator JSONArray(bool[] value)
        {
            return new JSONArray(value);
        }

        public JSONArray(Guid[] value)
        {
            _value = JsonConvert.SerializeObject(value);
        }
        public static implicit operator JSONArray(Guid[] value)
        {
            return new JSONArray(value);
        }

        public JSONArray(string[] value)
        {
            _value = JsonConvert.SerializeObject(value);
        }
        public static implicit operator JSONArray(string[] value)
        {
            return new JSONArray(value);
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
