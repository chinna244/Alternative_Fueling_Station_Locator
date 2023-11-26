using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_EF_Start.DataAccess;
using MVC_EF_Start.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace MVC_EF_Start.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
        }
        

        public ViewResult Question1()
        {
            // Query to select distinct first and last names of graduates with non-null names
            var graduates = dbContext.Projects
                .Where(pd => pd.Student != null && pd.Student.First_Name != null && pd.Student.Last_Name != null)
                .Select(pd => new
                {
                    First_Name = pd.Student.First_Name,
                    Last_Name = pd.Student.Last_Name
                })
                .Distinct()
                .ToList();

            return View(graduates);
        }

        public ViewResult Question2()
        {
            // Query to select distinct first and last names of students who published on a specific research topic
            var query = from Student in dbContext.students
                        where Student.ProjectDocuments.Any(pd => pd.Research_Topic == "Machine Learning" && pd.Student != null)
                        select new
                        {
                            First_Name = Student.First_Name,
                            Last_Name = Student.Last_Name
                        };

            var result = query.Distinct().ToList();

            return View(result);
        }

        public ViewResult Question3(int user_id, DateTime Given_Date)
        {
            // Query to select document titles and research topics based on user ID and date
            var query = from downloadInfo in dbContext.Downloads
                        where downloadInfo.Download_Date.Date == Given_Date.Date && downloadInfo.User.User_Id == user_id
                        select new
                        {
                            Document_Title = downloadInfo.ProjectDocument.Title,
                            Research_Topic = downloadInfo.ProjectDocument.Research_Topic
                        };

            var result = query.ToList();

            return View(result);

        }

        public ViewResult Question4()
        {
            // Query to select the top 2 research topics with the highest publication count
            var topResearchTopics = dbContext.Projects
                    .GroupBy(pd => pd.Research_Topic)
                    .OrderByDescending(group => group.Count())
                    .Take(2)
                    .Select(group => new
                    {
                        Research_Topic = group.Key,
                        Publication_Count = group.Count()
                    })
                    .ToList();

            return View(topResearchTopics);
        }

        public ViewResult Question5()
        {
            // Query to select the top 2 research topics with the highest download count
            var topDownloadedTopics = dbContext.Projects
                .OrderByDescending(pd => pd.DownloadInformations.Count)
                .Take(2)
                .Select(pd => new
                {
                    Research_Topic = pd.Research_Topic,
                    Download_Count = pd.DownloadInformations.Count
                })
                .ToList();

            return View(topDownloadedTopics);
        }


        public JsonResult Search(string City,string Fuel)
        {
            List<Stations> Station_details = new List<Stations>();
            if(Fuel=="ALL")
            {
                if (IsValidZipCode(City))
                {
                    Station_details = dbContext.Fuel_Stations
                       .Where(s => s.zip == City)
                       .OrderByDescending(s=>s.Date_Updated)
                       .ToList();
                }

                else
                {
                    Station_details = dbContext.Fuel_Stations
                        .Where(s => s.city == City)
                        .OrderByDescending(s => s.Date_Updated)
                        .ToList();
                }

            }
            else
            {
                if (IsValidZipCode(City))
                {
                    Station_details = dbContext.Fuel_Stations
                       .Where(s => s.zip == City && s.fuel_type_code == Fuel)
                       .OrderByDescending(s => s.Date_Updated)
                       .ToList();
                }

                else
                {
                    Station_details = dbContext.Fuel_Stations
                        .Where(s => s.city == City && s.fuel_type_code == Fuel)
                        .OrderByDescending(s => s.Date_Updated)
                        .ToList();
                }

            }


            return Json(Station_details);
        }

        public IActionResult Index()
        {
            station_details Stations_latest = new station_details();
            Stations_latest.Stations = dbContext.Fuel_Stations
                .OrderByDescending(s => s.Date_Updated)
                .Take(10)
                .ToList();
 

            return View(Stations_latest);

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Stations s)
        {
            s.Date_Updated = DateTime.Now;
            dbContext.Fuel_Stations.Add(s);

            dbContext.SaveChanges();

            TempData["AlertMessage"] = "Fuel station added successfully.";

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Stations s = dbContext.Fuel_Stations
                                   .Where(c => c.station_id == id)
                                   .First();

            return View(s);
        }

        [HttpPost]
        public ActionResult Edit(Stations s)
        {
            s.Date_Updated = DateTime.Now;
            dbContext.Fuel_Stations.Update(s);
            dbContext.SaveChanges();
            //await dbContext.SaveChangesAsync();

            TempData["AlertMessage"] = "Fuel station details are updated successfully.";

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult DeleteStation(int id)
        {
             var station = dbContext.Fuel_Stations.Find(id);
                     dbContext.Fuel_Stations.Remove(station);
                     dbContext.SaveChanges();

            return Json(new { success = true, message = "Station deleted successfully." });
        }
        static bool IsValidZipCode(string zipCode)
        {
            // US ZIP code pattern (5 digits)
            string pattern = @"^\d{5}$";

            // Optional: If you want to include ZIP+4 format (e.g., 12345-6789)
            // string pattern = @"^\d{5}(-\d{4})?$";

            Regex regex = new Regex(pattern);

            return regex.IsMatch(zipCode);
        }
        [HttpGet]
        public ActionResult Station_Details()
        {
            station_details Stations_latest = new station_details();
            Stations_latest.Stations = dbContext.Fuel_Stations
                .ToList();
            return Json(new { stations = Stations_latest });
        }

    }
}