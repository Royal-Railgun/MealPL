using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WW.MPLN.Core;
using System.Linq;

namespace WW.MPLN.Storage
{
    internal class MealPlanReader
    {
        private string _table = "meal_plans";
        private IMongoCollection<BsonDocument> _collection;

        public MealPlanReader()
        {
            var database = MongoBase.GetDataBase();
            _collection = database.GetCollection<BsonDocument>(_table);
        }

        public MealPlan Save(MealPlan plan)
        {
            plan.ID = MongoBase.AutoID("mealplan");
            _collection.InsertOne(plan.ToBsonDocument());
            return plan;
        }

        public void Update(MealPlan plan)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("ID", plan.ID);
            var option = new UpdateOptions();
            option.IsUpsert = true;
            var result = _collection.ReplaceOne(filter, plan.ToBsonDocument(), option);
        }

        public MealPlan GetPlan(long planID)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("ID", planID);
            var doc = _collection.Find<BsonDocument>(filter).FirstOrDefault();
            return FromBson(doc);
        }

        public IList<MealPlan> Query(string key, int page, int pageSize)
        {
            if (page < 0)
                page = 0;
            if (pageSize <= 0 || pageSize > 500)
                pageSize = 20;
            var filter = Builders<BsonDocument>.Filter.Empty;
            if (!string.IsNullOrEmpty(key))
            {
                BsonRegularExpression reg = new BsonRegularExpression(new Regex(string.Concat(".*", key, ".*"), RegexOptions.Compiled | RegexOptions.IgnoreCase));
                filter = filter & (Builders<BsonDocument>.Filter.Regex("Name", reg) | Builders<BsonDocument>.Filter.Regex("Tags", reg));
            }
            var query = _collection.Find<BsonDocument>(filter).Skip(page * pageSize).Limit(pageSize).ToCursor();
            List<MealPlan> result = new List<MealPlan>();
            foreach (var p in query.ToEnumerable())
            {
                var pro = FromBson(p);
                result.Add(pro);
            }
            query.Dispose();
            return result;
        }

        public static MealPlan FromBson(BsonDocument doc)
        {
            if (doc == null)
                return null;
            if (!doc.ExistFiled("ID"))
                return null;
            return new MealPlan()
            {
                ID = doc.TryReadLong("ID"),
                Name = doc.TryReadString("Name"),
                Tags = doc.TryReadArray("Tags").ToArray(),
                Description = doc.TryReadString("Description"),
                Createtime = doc.TryReadDate("Createtime").Value,
                UpdateTime = doc.TryReadDate("UpdateTime").Value,
            };
        }
    }
}