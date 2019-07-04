using System;
using System.Collections.Generic;
using WW.MPLN.Core;

namespace WW.MPLN.Storage
{
    public class MealPlanRepository : IMealPlanRepository
    {
        private MealPlanReader _dbReader;

        public MealPlanRepository()
        {
            _dbReader = new MealPlanReader();
        }

        public long Create(MealPlan item)
        {
            _dbReader.Save(item);
            return item.ID;
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public MealPlan Get(long id)
        {
            return _dbReader.GetPlan(id);
        }

        public IList<MealPlan> Query(string key, int page, int pagesize)
        {
            return _dbReader.Query(key, page, pagesize);
        }

        public MealPlan Update(MealPlan item)
        {
            _dbReader.Update(item);
            return item;
        }
    }
}