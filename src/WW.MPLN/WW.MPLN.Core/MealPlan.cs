using System;

namespace WW.MPLN.Core
{
    public class MealPlan
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string[] Tags { get; set; }
        public string Description { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}