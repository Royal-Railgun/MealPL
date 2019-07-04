using System.Collections.Generic;
using WW.MPLN.Core;

namespace WW.MPLN.Admin.Models
{
    public class MealPlanQueryModel
    {
        public string Key { get; set; }
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 20;
    }

    public class MealPlanResultModel
    {
        public string Key { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public IList<MealPlan> Items { get; set; }
        public int ItemCount { get; set; }
    }
}