using CustomGenerics.Structures;
using LabAVL_1170919.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LabAVL_1170919.Helpers
{
    public class Storage
    {
        public static Storage _instance = null;

        public static Storage Instance
        {
            get
            {
                if (_instance == null) _instance = new Storage();
                return _instance;
            }
        }
        public BinaryTree<TreeMedicine> binaryTree = new BinaryTree<TreeMedicine>();
        public List<MedicineModel> medicineList = new List<MedicineModel>();
        public Dictionary<string, Client> orders = new Dictionary<string, Client>();
        public string actualClient;
        public HttpPostedFileBase file;
    }
}