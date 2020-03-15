using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabAVL_1170919.Models
{
    public class Client
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Nit { get; set; }
        public List<MedicineModel> Medicines { get; set; }
        public double Debt { get; set; }

        public Client()
        {
            Name = "";
            Address = "";
            Nit = "";
            Medicines = null;
            Debt = 0;
        }
    }
}