using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace WW.MPLN.Storage
{
    internal class MongoBase
    {
        private static string _dataBaseName = "htq_mealplan_dev";
        private static string _connectionString = "mongodb://172.16.40.140";

        public static UpdateOptions UpsertOption = new UpdateOptions();
        public static FindOneAndUpdateOptions<BsonDocument> UpsertOption_Update = new FindOneAndUpdateOptions<BsonDocument>() { IsUpsert = true };

        static MongoBase()
        {
            UpsertOption.IsUpsert = true;
        }

        public static IMongoDatabase GetDataBase(string db = null)
        {
            if (string.IsNullOrEmpty(db))
                db = _dataBaseName;
            var client = new MongoClient(_connectionString);
            return client.GetDatabase(db);
        }

        public static IEnumerable<BsonDocument> ReadCollection(string table, string db = null)
        {
            var database = GetDataBase(db);
            var collection = database.GetCollection<BsonDocument>(table);
            var query = collection.Find<BsonDocument>(new BsonDocument()).ToCursor();
            return query.ToEnumerable();
        }

        public static IEnumerable<BsonDocument> ReadCollection(int page, int pagesize, string table, string db = null)
        {
            var database = GetDataBase(db);
            var collection = database.GetCollection<BsonDocument>(table);
            var query = collection.Find<BsonDocument>(new BsonDocument()).Skip(page * pagesize).Limit(pagesize);
            return query.ToEnumerable();
        }

        public static void SaveData(string table, object data, string db = null)
        {
            var database = GetDataBase(db);
            var collection = database.GetCollection<BsonDocument>(table);
            collection.InsertOne(data.ToBsonDocument());
        }

        public static long AutoID(string key, string db = null)
        {
            var database = GetDataBase(db);
            var collection = database.GetCollection<BsonDocument>("_ids");
            var filter = Builders<BsonDocument>.Filter.Eq("key", key);
            var update = Builders<BsonDocument>.Update.Inc("id", (long)1);
            var result = collection.FindOneAndUpdate(filter, update, UpsertOption_Update);
            return result.TryReadLong("id");
        }
    }
}