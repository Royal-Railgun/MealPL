using System;

namespace WW.MPLN.Core
{
    public class MealItem
    {
        public long ID { get; set; }
        public long PlanID { get; set; }
        public string Name { get; set; }
        public string NBDID { get; set; }
        public double Count { get; set; }
        public string Unit { get; set; }
        public string[] Tags { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime Createtime { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public enum MealTypes
    {
        Breakfast,
        Snack1,
        Lunch,
        Snack2,
        Dinner
    }

    public enum MealUnits
    {
        Bar,
        Bottle,
        Box,
        Container,
        Cup,
        Fillet,
        FL_OZ,
        L,
        Large,
        Medium,
        ML,
        OZ,
        Packet,
        Piece,
        Slice,
        Small,
        Tablespoon,
        Teaspoon,
        G,
    }
}