using System;
using System.Collections.Generic;
using WW.MPLN.Core;

namespace WW.MPLN.Admin.Models
{
    public class MealPlanViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }
        public IList<MealItem> MealItems { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}