using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabAVL_1170919.Models
{
    public class MedicineModel : TreeMedicine
    {
        public static int ID { get; set; }
        public string Description { get; set; }
        public string Producer { get; set; }
        public double Price { get; set; }

        public MedicineModel()
        {
            Id = ID;
            Description = "";
            Producer = "";
            Price = 0;
        }

    }
}