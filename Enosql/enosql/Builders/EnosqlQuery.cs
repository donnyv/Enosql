using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using enosql.JSON;

namespace enosql.Builders
{
    public static class EnosqlQuery
    {
        /// <summary>
        /// Assumes the object property is an array and only returns collections where all supplied values are matched.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string All(string name, JSONArray values)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            return "{\"" + name + "\": {\"$all\":" + values.ToString() + "} }";
        }

        /// <summary>
        /// Multiple queries can be combined together. 
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public static string And(IEnumerable<string> queries)
        {
            if (queries == null)
            {
                throw new ArgumentNullException("queries");
            }

            var queriesJSON = new StringBuilder();
            var q = queries.ToArray();
            for (int i = 0, l = q.Length; i < l; i++)
            {
                queriesJSON.Append(StripCurlies(q[i]));

                if ((i + 1) != l)
                    queriesJSON.Append(",");
            }

            return "{\"$and\": { " + queriesJSON.ToString() + "} }";
        }
        public static string And(params string[] queries)
        {
            return And((IEnumerable<string>)queries);
        }

        static string StripCurlies(string q)
        {
            var t = q.Substring(1);
            return t.Remove(t.Length - 1);
        }

        /// <summary>
        /// Assumes that the model property is an array and searches for the query value in the array.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Contains(string name, JSONValue value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return "{\"" + name + "\": {\"$contains\":" + value.ToString() + "} }";
        }

        /// <summary>
        /// Performs a strict equality test using ===.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EQ(string name, JSONValue value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return "{\"" + name + "\": {\"$equal\":" + value.ToString() + "} }";
        }

        /// <summary>
        /// If the attribute in the object is an array then the query value is searched for in the array.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string EQ(string name, JSONArray values)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            return "{\"" + name + "\":" + values.ToString() + "}";
        }

        /// <summary>
        /// Performs a case insensitive search
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EQi(string name, JSONValue value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return "{\"" + name + "\": {\"$regex\": \"" + value.ToString("/^{0}$/i") + "\" } }";
        }

        /// <summary>
        /// Checks for the existence of a named element. Can be supplied either true or false.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Exists(string name, bool value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            return "{\"" + name + "\": {\"$exists\":" + new JSONValue(value).ToString() + "} }";
        }

        /// <summary>
        /// Tests that the value of the named element is greater than some value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GT(string name, JSONValue value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return "{\"" + name + "\": {\"$gt\":" + value.ToString() + "} }";
        }

        /// <summary>
        /// Tests that the value of the named element is greater than or equal to some value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GTE(string name, JSONValue value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return "{\"" + name + "\": {\"$gte\":" + value.ToString() + "} }";
        }

        /// <summary>
        /// An array of possible values can be supplied, a collection will be returned if any of the supplied values is matched
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string In(string name, JSONArray values)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            return "{\"" + name + "\" : {\"$in\":" + values.ToString() + "} }";
        }

        /// <summary>
        /// Tests that the value of the named element is less than some value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LT(string name, JSONValue value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return "{\"" + name + "\": {\"$lt\":" + value.ToString() + "} }";
        }

        /// <summary>
        /// Tests that the value of the named element is less than or equal to some value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LTE(string name, JSONValue value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return "{\"" + name + "\": {\"$lte\":" + value.ToString() + "} }";
        }

        /// <summary>
        /// Checks if the object attribute matches the supplied regular expression
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Matches(string name, string value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return "{\"" + name + "\": {\"$regex\":" + value.ToString() + "} }";
        }

        /// <summary>
        /// "Not equal", the opposite of "EQ", returns all collections which don't have the query value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string NE(string name, JSONValue value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return "{\"" + name + "\": {\"$ne\":" + value.ToString() + "} }";
        }

        /// <summary>
        /// The opposite of "And".
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public static string Not(IEnumerable<string> queries)
        {
            if (queries == null)
            {
                throw new ArgumentNullException("queries");
            }

            var queriesJSON = new StringBuilder();
            var q = queries.ToArray();
            for (int i = 0, l = q.Length; i < l; i++)
            {
                queriesJSON.Append(StripCurlies(q[i]));

                if ((i + 1) != l)
                    queriesJSON.Append(",");
            }

            return "{\"$not\": { " + queriesJSON.ToString() + "} }";
        }
        public static string Not(params string[] queries)
        {
            return Not((IEnumerable<string>)queries);
        }

        /// <summary>
        /// "Not in", the opposite of "In". A collection will be returned if none of the supplied values is matched
        /// </summary>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string NotIn(string name, JSONArray values)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            return "{\"" + name + "\": {\"$nin\":" + values.ToString() + "} }";
        }

        /// <summary>
        /// Tests that at least one of the subqueries is true. 
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        public static string Or(IEnumerable<string> queries)
        {
            if (queries == null)
            {
                throw new ArgumentNullException("queries");
            }

            var queriesJSON = new StringBuilder();
            var q = queries.ToArray();
            for (int i = 0, l = q.Length; i < l; i++)
            {
                queriesJSON.Append(StripCurlies(q[i]));

                if ((i + 1) != l)
                    queriesJSON.Append(",");
            }

            return "{\"$or\": { " + queriesJSON.ToString() + "} }";
        }
        public static string Or(params string[] queries)
        {
            return Or((IEnumerable<string>)queries);
        }

        /// <summary>
        /// If you need to perform multiple queries on the same name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="queries"></param>
        /// <returns></returns>
        public static string Or(string name, IEnumerable<JSONValue> queries)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (queries == null)
            {
                throw new ArgumentNullException("queries");
            }

            var queriesJSON = new StringBuilder();
            foreach (var q in queries)
            {
                queriesJSON.Append("{\"" + name + "\":" + q.ToString() + "},");
            }

            var qJSON = queriesJSON.ToString();
            return "{\"$or\": [ " + qJSON.Remove(qJSON.Length - 1) + "] }";
        }
        public static string Or(string name, params JSONValue[] queries)
        {
            return Or(name, (IEnumerable<JSONValue>)queries);
        }

        /* 
         * ***** Non mongodb queries *****
         */
        /// <summary>
        /// To check if a value is in-between 2 query values.
        /// Supply min and max values.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static string Between(string name, JSONValue min, JSONValue max)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (min == null)
            {
                throw new ArgumentNullException("start");
            }
            if (max == null)
            {
                throw new ArgumentNullException("end");
            }

            return "{\"" + name + "\": {\"$between\":[" + min.ToString() + "," + max.ToString() + "]} }";
        }

        /// <summary>
        /// Assumes the object attribute is a string and checks if the supplied query value is a substring of the property. 
        /// Uses indexOf rather than regex for performance reasons.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Like(string name, JSONValue value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return "{\"" + name + "\": {\"$like\":" + value.ToString() + "} }";
        }

        /// <summary>
        /// The same as "Like" but performs a case insensitive search using indexOf and toLowerCase (still faster than Regex)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string LikeI(string name, JSONValue value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return "{\"" + name + "\": {\"$likeI\":" + value.ToString() + "} }";
        }

        /// <summary>
        /// Assumes the object property has a length (i.e. is either an array or a string). 
        /// Only returns objects the object property's length matches the supplied values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Size(string name, JSONValue value)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return "{\"" + name + "\": {\"$size\":" + value.ToString() + "} }";
        }
    }

    //public static class EnosqlQuery<TJson>
    //{
    //    public static string EQ<TMember>(Expression<Func<TDocument, TMember>> memberExpression, TMember value)
    //    {
    //        return new EnosqlQuery<TJson>().EQ(memberExpression, value);
    //    }
    //}
}
