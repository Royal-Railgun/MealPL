using System.Collections.Generic;

namespace WW.MPLN.Core
{
    public interface IMealPlanRepository
    {
        long Create(MealPlan item);

        MealPlan Update(MealPlan item);

        void Delete(long id);

        MealPlan Get(long id);

        IList<MealPlan> Query(string key, int page, int pagesize);
    }
}