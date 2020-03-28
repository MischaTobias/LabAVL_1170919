using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabAVL_1170919.Models
{
    public class TreeMedicine
    {
        public static int ID { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }

        public TreeMedicine()
        {
            Id = ID;
            Name = "";
            Stock = 0;
        }

        public static Comparison<TreeMedicine> CompareByName = delegate (TreeMedicine med1, TreeMedicine med2)
        {
            return med2.Name.CompareTo(med1.Name);
        };

        public int CompareTo(object obj)
        {
            return this.Id.CompareTo(((TreeMedicine)obj).Id);
        }
    }
}
