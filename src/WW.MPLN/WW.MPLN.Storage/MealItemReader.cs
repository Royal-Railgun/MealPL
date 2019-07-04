using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using WW.MPLN.Core;

namespace WW.MPLN.Storage
{
    internal class MealItemReader
    {
        private string _table = "meal_items";
        private IMongoCollection<BsonDocument> _collection;

        public MealItemReader()
        {
            var database = MongoBase.GetDataBase();
            _collection = database.GetCollection<BsonDocument>(_table);
        }

        public void Save(IList<MealItem> items)
        {
            foreach (var p in items)
            {
                p.ID = MongoBase.AutoID("mealitem");
                _collection.InsertOne(p.ToBsonDocument());
            }
        }

        public MealItem Save(MealItem item)
        {
            item.ID = MongoBase.AutoID("mealitem");
            _collection.InsertOne(item.ToBsonDocument());
            return item;
        }

        public void Update(MealItem item)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("ID", item.ID);
            var option = new UpdateOptions();
            option.IsUpsert = true;
            var result = _collection.ReplaceOne(filter, item.ToBsonDocument(), option);
        }

        public void Delete(long itemID)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("ID", itemID);
            _collection.DeleteOne(filter);
        }

        public MealItem Read(long itemID)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("ID", itemID);
            var doc = _collection.Find<BsonDocument>(filter).FirstOrDefault();
            return FromBson(doc);
        }

        public IList<MealItem> ReadByPlan(long planID)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("PlanID", planID);
            var query = _collection.Find<BsonDocument>(filter).ToCursor();
            List<MealItem> result = new List<MealItem>();
            foreach (var p in query.ToEnumerable())
            {
                var pro = FromBson(p);
                result.Add(pro);
            }
            query.Dispose();
            return result;
        }

        public static MealItem FromBson(BsonDocument doc)
        {
            if (doc == null)
                return null;
            if (!doc.ExistFiled("ID"))
                return null;
            return new MealItem()
            {
                ID = doc.TryReadLong("ID"),
                PlanID = doc.TryReadLong("PlanID"),
                Name = doc.TryReadString("Name"),
                Count = doc.TryReadDouble("Count"),
                NBDID = doc.TryReadString("NBDID"),
                Unit = doc.TryReadString("Unit"),
                Tags = doc.TryReadArray("Tags").ToArray(),
                Type = doc.TryReadString("Type"),
                Description = doc.TryReadString("Description"),
                Createtime = doc.TryReadDate("Createtime").Value,
                UpdateTime = doc.TryReadDate("UpdateTime").Value,
            };
        }
    }
}