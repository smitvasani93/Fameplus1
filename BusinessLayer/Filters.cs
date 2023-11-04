using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace BusinessLayer.Models
{
    public class Filters
    {
        public enum GroupOp
        {
            AND,
            OR
        }
        public enum Operations
        {
            eq, // "equal"
            ne, // "not equal"
            lt, // "less"
            le, // "less or equal"
            gt, // "greater"
            ge, // "greater or equal"
            bw, // "begins with"
            bn, // "does not begin with"
                //in, // "in"
                //ni, // "not in"
            ew, // "ends with"
            en, // "does not end with"
            cn, // "contains"
            nc  // "does not contain"
        }
        public class Rule
        {
            public string field { get; set; }
            public Operations op { get; set; }
            public string data { get; set; }
        }

        public GroupOp groupOp { get; set; }
        public List<Rule> rules { get; set; }
        private static readonly string[] FormatMapping = {
        "(it.{0} = @p{1})",                 // "eq" - equal
        "(it.{0} <> @p{1})",                // "ne" - not equal
        "(it.{0} < @p{1})",                 // "lt" - less than
        "(it.{0} <= @p{1})",                // "le" - less than or equal to
        "(it.{0} > @p{1})",                 // "gt" - greater than
        "(it.{0} >= @p{1})",                // "ge" - greater than or equal to
        "(it.{0} LIKE (@p{1}+'%'))",        // "bw" - begins with
        "(it.{0} NOT LIKE (@p{1}+'%'))",    // "bn" - does not begin with
        "(it.{0} LIKE ('%'+@p{1}))",        // "ew" - ends with
        "(it.{0} NOT LIKE ('%'+@p{1}))",    // "en" - does not end with
        "(it.{0} LIKE ('%'+@p{1}+'%'))",    // "cn" - contains
        "(it.{0} NOT LIKE ('%'+@p{1}+'%'))" //" nc" - does not contain
    };
        internal ObjectQuery<T> FilterObjectSet<T>(ObjectQuery<T> inputQuery) where T : class
        {
            if (rules.Count <= 0)
                return inputQuery;

            var sb = new StringBuilder();
            var objParams = new List<ObjectParameter>(rules.Count);

            foreach (Rule rule in rules)
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(rule.field);
                if (propertyInfo == null)
                    continue; // skip wrong entries

                if (sb.Length != 0)
                    sb.Append(groupOp);

                var iParam = objParams.Count;
                sb.AppendFormat(FormatMapping[(int)rule.op], rule.field, iParam);

                // TODO: Extend to other data types
                objParams.Add(String.Compare(propertyInfo.PropertyType.FullName,
                                             "System.Int32", StringComparison.Ordinal) == 0
                                  ? new ObjectParameter("p" + iParam, Int32.Parse(rule.data))
                                  : new ObjectParameter("p" + iParam, rule.data));
            }

            ObjectQuery<T> filteredQuery = inputQuery.Where(sb.ToString());
            foreach (var objParam in objParams)
                filteredQuery.Parameters.Add(objParam);

            return filteredQuery;
        }
    }
}