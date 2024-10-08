﻿using LabAVL_1170919.Helpers;
using LabAVL_1170919.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Text;

namespace LabAVL_1170919.Controllers
{
    public class MedicineController : Controller
    {
        // GET: Medicine
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            try
            {
                Storage.Instance.file = file;
                StreamReader streamReader = new StreamReader(Storage.Instance.file.InputStream);
                var line = streamReader.ReadLine();
                if (line.Split(',')[0] == "id")
                {
                    line = streamReader.ReadLine();
                }
                var medicineData = new List<string>();
                while (line != null)
                {
                    while (line != null)
                    {
                        var comilla = line.IndexOf('"');
                        var coma = line.IndexOf(',');

                        if (coma < comilla)
                        {
                            var data = line.Substring(0, coma);
                            line = line.Remove(0, coma + 1);
                            medicineData.Add(data);
                        }
                        else
                        {
                            if (comilla < 0)
                            {
                                if (line.IndexOf('$') < 1 && line.IndexOf('$') > -1)
                                {
                                    line = line.Remove(0, 1);
                                }
                                comilla = line.Length;
                                coma = line.IndexOf(',');
                                string data = "";
                                if (coma < 0)
                                {
                                    data = line;
                                    line = null;
                                }
                                else
                                {
                                    data = line.Substring(0, coma);
                                    line = line.Remove(0, coma + 1);
                                }
                                medicineData.Add(data);
                            }
                            else
                            {
                                line = line.Remove(0, 1);
                                comilla = line.IndexOf('"');
                                var data = line.Substring(0, comilla);
                                line = line.Remove(0, comilla + 2);
                                medicineData.Add(data);
                            }
                        }
                    }
                    MedicineModel newMedicine = new MedicineModel
                    {
                        Id = int.Parse(medicineData[0]),
                        Name = medicineData[1],
                        Description = medicineData[2],
                        Producer = medicineData[3],
                        Price = Convert.ToDouble(medicineData[4]),
                        Stock = int.Parse(medicineData[5])
                    };
                    Storage.Instance.medicineList.Add(newMedicine);
                    TreeMedicine medicineNode = new TreeMedicine
                    {
                        Id = int.Parse(medicineData[0]),
                        Name = medicineData[1],
                        Stock = int.Parse(medicineData[5])
                    };
                    Storage.Instance.binaryTree.AddMedicine(medicineNode, TreeMedicine.CompareByName);
                    line = streamReader.ReadLine();
                    medicineData = new List<string>();
                }
                streamReader.Close();
                return RedirectToAction("ClientInfoInput");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult ClientInfoInput()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ClientInfoInput(FormCollection collection)
        {
            try
            {
                if (Storage.Instance.orders.ContainsKey(collection["Name"]))
                {
                    Storage.Instance.actualClient = collection["Name"];
                }
                else
                {
                    Restock();
                    Client newClient = new Client()
                    {
                        Name = collection["Name"],
                        Address = collection["Address"],
                        Nit = collection["Nit"]
                    };
                    Storage.Instance.orders.Add(newClient.Name, newClient);
                    Storage.Instance.actualClient = newClient.Name;
                }
                return RedirectToAction("ShowMedList");
            }
            catch
            {
                return View();
            }
        }

        private void Restock()
        {
            Random random = new Random();
            var newStock = random.Next() % 15;
            foreach (var med in Storage.Instance.medicineList)
            {
                if (med.Stock == 0)
                {
                    while (newStock == 0)
                    {
                        newStock = random.Next() % 15;
                    }
                    med.Stock = newStock;
                    TreeMedicine medicine = new TreeMedicine()
                    {
                        Id = med.Id,
                        Name = med.Name,
                        Stock = newStock
                    };
                    Storage.Instance.binaryTree.AddMedicine(medicine, TreeMedicine.CompareByName);
                }
            }
        }

        public ActionResult ShowMedList(int? page, string search, int? pathing)
        {
            int path = (pathing ?? 1);
            var list = (Storage.Instance.binaryTree.GetList(path)).Select(x => x.Medicine).ToList();

            if (search != "" && search != null)
            {
                list = list.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
            }
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            var medList = new List<MedicineModel>();
            foreach (var med in list)
            {
                foreach (var item in Storage.Instance.medicineList)
                {
                    if (med.Id == item.Id)
                    {
                        medList.Add(item);
                    }
                }
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(medList.ToPagedList(pageNumber, pageSize));
            //Obtenido de https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application#add-paging
        }

        public ActionResult AddToCart(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddToCart(int id, FormCollection collection)
        {
            //Obtenido de https://stackoverflow.com/questions/43561214/display-error-message-on-the-view-from-controller-asp-net-mvc-5
            var med = Storage.Instance.medicineList[id - 1].Stock;
            if (collection["Stock"] == "")
            {
                ModelState.AddModelError("Stock", "Please enter a number of medicines you would like to order");
                return View("AddToCart");
            }
            var ordered = int.Parse(collection["Stock"]);
            if (ordered > med)
            {
                ModelState.AddModelError("Stock", "The quantity you want is more than what is in stock, please input a lower quantity");
                return View("AddToCart");
            }
            else if (ordered == med)
            {
                Storage.Instance.medicineList[id - 1].Stock = 0;
                CustomGenerics.Structures.BinaryTreeNode<TreeMedicine> node = new CustomGenerics.Structures.BinaryTreeNode<TreeMedicine>()
                { Father = null, LeftSon = null, RightSon = null, Medicine = Storage.Instance.medicineList[id - 1] };
                Storage.Instance.binaryTree.Delete(Storage.Instance.binaryTree.root, node, TreeMedicine.CompareByName);
            }
            else
            {
                Storage.Instance.medicineList[id - 1].Stock = med - ordered;
                TreeMedicine medicine = new TreeMedicine()
                {
                    Id = id,
                    Name = Storage.Instance.medicineList[id - 1].Name,
                    Stock = Storage.Instance.medicineList[id - 1].Stock
                };
                Storage.Instance.binaryTree.TakeMed(medicine, TreeMedicine.CompareByName);
            }
            MedicineModel newmedicine = new MedicineModel()
            {
                Id = Storage.Instance.medicineList[id - 1].Id,
                Name = Storage.Instance.medicineList[id - 1].Name,
                Producer = Storage.Instance.medicineList[id -1].Producer,
                Price = Storage.Instance.medicineList[id -1].Price,
                Stock = ordered
            };
            Storage.Instance.orders[Storage.Instance.actualClient].Medicines.Add(newmedicine);
            Storage.Instance.orders[Storage.Instance.actualClient].Debt += Storage.Instance.medicineList[id - 1].Price * ordered;  
            return RedirectToAction("ShowMedList");
        }

        public ActionResult ShowOrder()
        {
            return View(Storage.Instance.orders[Storage.Instance.actualClient]);
        }

        public ActionResult Close()
        {
            return View();
        }

        public FileResult Export()
        {
            //Obtenido de https://www.aspsnippets.com/Articles/Export-to-CSV-in-ASPNet-MVC.aspx
            var data = new StringBuilder();
            data.AppendLine("Id,Name,Description,Producer,Price,Stock");
            foreach (var med in Storage.Instance.medicineList)
            {
                data.AppendLine(med.Id + "," + med.Name + "," + med.Description + "," + med.Producer + "," + med.Price + "," + med.Stock);
            }
            return File(Encoding.UTF8.GetBytes(data.ToString()), "text/csv", "NewInventory.csv");
        }

        public ActionResult End()
        {
            return View();
        }
    }
}
