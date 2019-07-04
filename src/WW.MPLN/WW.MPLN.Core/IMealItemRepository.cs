using System.Collections.Generic;

namespace WW.MPLN.Core
{
    public interface IMealItemRepository
    {
        long Create(MealItem item);

        void Create(IList<MealItem> items);

        MealItem Update(MealItem Item);

        void Delete(long itemID);

        IList<MealItem> RetrieveByPlan(long plan);

        MealItem Retrieve(long id);
    }
}