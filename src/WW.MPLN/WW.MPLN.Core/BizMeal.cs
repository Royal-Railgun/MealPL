using System;
using System.Collections.Generic;

namespace WW.MPLN.Core
{
    public class BizMeal
    {
        private IMealPlanRepository _panRepository;
        private IMealItemRepository _itemRepository;

        public BizMeal(IMealPlanRepository planRepository, IMealItemRepository itemRepository)
        {
            _panRepository = planRepository;
            _itemRepository = itemRepository;
        }

        public long CreatePlan(string name, string description, string[] tags, IList<MealItem> mealItems)
        {
            MealPlan plan = new MealPlan()
            {
                Description = description,
                Name = name,
                Tags = tags,
                Createtime = DateTime.Now,
                UpdateTime = DateTime.Now,
            };
            var planid = _panRepository.Create(plan);
            foreach (var item in mealItems)
            {
                item.PlanID = planid;
                item.Createtime = DateTime.Now;
                item.UpdateTime = DateTime.Now;
            }
            _itemRepository.Create(mealItems);
            return planid;
        }

        public bool UpdatePlan(MealPlan item)
        {
            var editItem = _panRepository.Get(item.ID);
            if (editItem == null)
                return false;
            editItem.Name = item.Name;
            editItem.Description = item.Description;
            editItem.Tags = item.Tags;
            editItem.UpdateTime = DateTime.Now;
            item.UpdateTime = DateTime.Now;
            _panRepository.Update(item);
            return true;
        }

        public bool UpdateItem(MealItem item)
        {
            var editItem = _itemRepository.Retrieve(item.ID);
            if (editItem == null)
                return false;
            editItem.Name = item.Name;
            editItem.Tags = item.Tags;
            editItem.Unit = item.Unit;
            editItem.NBDID = item.NBDID;
            editItem.Description = item.Description;
            editItem.Count = item.Count;
            editItem.UpdateTime = DateTime.Now;
            _itemRepository.Update(editItem);
            return true;
        }

        public bool CreateMealItem(MealItem item)
        {
            return _itemRepository.Create(item) > 0;
        }

        public IList<MealPlan> Query(string key, int page, int pagesize)
        {
            return _panRepository.Query(key, page, pagesize);
        }

        public MealPlan GetPlan(long planID)
        {
            return _panRepository.Get(planID);
        }

        public IList<MealItem> GetMealItems(long planID)
        {
            return _itemRepository.RetrieveByPlan(planID);
        }

        public MealItem GetMealItem(long itemID)
        {
            return _itemRepository.Retrieve(itemID);
        }

        public bool DeleteMealItem(long itemID)
        {
            _itemRepository.Delete(itemID);
            return true;
        }
    }
}