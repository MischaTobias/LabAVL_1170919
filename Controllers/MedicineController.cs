using LabAVL_1170919.Helpers;
using LabAVL_1170919.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                StreamReader streamReader = new StreamReader(file.InputStream);
                var line = streamReader.ReadLine();
                line = streamReader.ReadLine();
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
                                if (line.Contains('$'))
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
                }
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
                Client newClient = new Client()
                {
                    Name = collection["Name"],
                    Address = collection["Address"],
                    Nit = collection["Nit"]
                };
                Storage.Instance.client = newClient;
                return RedirectToAction("ShowMedList");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ShowMedList()
        {
            return View();
        }
    }
}
