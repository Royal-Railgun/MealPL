using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WW.MPLN.Storage
{
    public static class MongoExtensions
    {
        public static string TryReadString(this BsonDocument doc, string name)
        {
            if (!doc.ExistFiled(name) || doc[name].IsBsonNull)
                return string.Empty;
            return doc[name].ToString();
        }

        public static bool TryReadBool(this BsonDocument doc, string name)
        {
            if (!doc.ExistFiled(name) || doc[name].IsBsonNull)
                return false;
            return (bool)doc[name];
        }

        public static int TryReadInt(this BsonDocument doc, string name)
        {
            if (!doc.ExistFiled(name) || doc[name].IsBsonNull)
                return 0;
            try
            {
                return (int)doc[name];
            }
            catch { return 0; }
        }

        public static double TryReadDouble(this BsonDocument doc, string name)
        {
            if (!doc.ExistFiled(name) || doc[name].IsBsonNull)
                return 0;
            try
            {
                return double.Parse(doc[name].ToString());
            }
            catch { return 0; }
        }

        public static long TryReadLong(this BsonDocument doc, string name)
        {
            if (!doc.ExistFiled(name) || doc[name].IsBsonNull)
                return 0;
            return (long)doc[name];
        }

        public static T TryReadObject<T>(this BsonDocument doc, string name, Func<BsonDocument, T> ctor)
        {
            if (!doc.ExistFiled(name) || doc[name].IsBsonNull)
                return default(T);
            var attr = doc[name] as BsonDocument;
            if (attr == null)
                return default(T);
            return ctor(attr);
        }

        public static IEnumerable<string> TryReadArray(this BsonDocument doc, string name)
        {
            if (!doc.ExistFiled(name) || doc[name].IsBsonNull || !(doc[name].IsBsonArray))
                yield break;
            foreach (var p in (BsonArray)doc[name])
            {
                yield return p.ToString();
            }
        }

        public static IEnumerable<T> TryReadArray<T>(this BsonDocument doc, string name, Func<BsonValue, T> ctor)
        {
            if (!doc.ExistFiled(name) || doc[name].IsBsonNull || !(doc[name].IsBsonArray))
                yield break;
            foreach (var p in (BsonArray)doc[name])
            {
                yield return ctor(p);
            }
        }

        public static DateTime? TryReadDate(this BsonDocument doc, string name)
        {
            string filedName = name;
            if (!doc.ExistFiled(name, out filedName) || doc[filedName].IsBsonNull)
                return null;
            var value = doc[filedName];
            if (value.IsValidDateTime)
                return ((BsonDateTime)doc[filedName]).ToUniversalTime();
            return null;
        }

        public static bool ExistFiled(this BsonDocument doc, string name)
        {
            if (doc == null)
                return false;
            if (doc.IndexOfName(name) >= 0)
                return true;
            return false;
        }

        public static bool ExistFiled(this BsonDocument doc, string name, out string fieldName)
        {
            fieldName = name;
            if (doc == null)
                return false;
            var item = doc.Elements.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (item == null || item.Name == null)
                return false;
            fieldName = item.Name;
            return true;
        }
    }
}