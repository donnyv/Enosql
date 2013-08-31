using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enosql.JSON
{
    public class JSONValue
    {
        string _value = string.Empty;

        public JSONValue(int value)
        {
            _value = value.ToString();
        }
        public static implicit operator JSONValue(int value)
        {
            return new JSONValue(value);
        }

        public JSONValue(double value)
        {
            _value = value.ToString();
        }
        public static implicit operator JSONValue(double value)
        {
            return new JSONValue(value);
        }

        public JSONValue(bool value)
        {
            _value = value.ToString().ToLower();
        }
        public static implicit operator JSONValue(bool value)
        {
            return new JSONValue(value);
        }

        public JSONValue(Guid value)
        {
            _value = "\"" + value.ToString().ToUpper() + "\"";
        }
        public static implicit operator JSONValue(Guid value)
        {
            return new JSONValue(value);
        }

        public JSONValue(string value)
        {
            _value = "\"" + value + "\"";
        }
        public static implicit operator JSONValue(string value)
        {
            return new JSONValue(value);
        }

        public override string ToString()
        {
            return _value;
        }

        public string ToString(string format = "")
        {
            return format == string.Empty ? _value : string.Format(format, _value.Replace("\"", ""));
        }
    }
}
