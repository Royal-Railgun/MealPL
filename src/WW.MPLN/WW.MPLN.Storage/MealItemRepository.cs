using System.Collections.Generic;
using WW.MPLN.Core;

namespace WW.MPLN.Storage
{
    public class MealItemRepository : IMealItemRepository
    {
        private MealItemReader _dbReader;

        public MealItemRepository()
        {
            _dbReader = new MealItemReader();
        }

        public long Create(MealItem item)
        {
            return _dbReader.Save(item).ID;
        }

        public void Create(IList<MealItem> items)
        {
            _dbReader.Save(items);
        }

        public void Delete(long itemID)
        {
            _dbReader.Delete(itemID);
        }

        public MealItem Retrieve(long id)
        {
            return _dbReader.Read(id);
        }

        public IList<MealItem> RetrieveByPlan(long plan)
        {
            return _dbReader.ReadByPlan(plan);
        }

        public MealItem Update(MealItem Item)
        {
            _dbReader.Update(Item);
            return Item;
        }
    }
}