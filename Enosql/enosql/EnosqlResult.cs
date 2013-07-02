using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace enosql
{
    public class EnosqlResult
    {
        public bool IsError { get; set; }
        public string Msg { get; set; }
        public string Json { get; set; }

        public dynamic DynamicJson {
            get
            {
                if(this.IsError)
                    return null;

                if(string.IsNullOrEmpty(this.Json))
                    return null;

                return JsonConvert.DeserializeObject<dynamic>(this.Json);
            }
        }
    }

    public class EnosqlResult<T> : EnosqlResult
    {
        public EnosqlResult(EnosqlResult ret)
        {
            this.IsError = ret.IsError;
            this.Msg = ret.Msg;
            this.Json = ret.Json;
        }
        public List<T> Data {
            get
            {
                if (this.IsError)
                    return null;

                if (string.IsNullOrEmpty(this.Json))
                    return null;

                return JsonConvert.DeserializeObject<List<T>>(this.Json);
            }
        }
    }
}
