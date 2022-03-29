using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mission13.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mission13.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private BowlingLeagueDbContext bowler { get; set; }

        //contstructor
        public HomeController(BowlingLeagueDbContext someName)
        {
            bowler = someName;
        }


        public IActionResult Index(string teamName)
        {
            // If a key-value pair of "id" exists
            HttpContext.Session.Remove("id");

            // assign teamName in this ViewBag.TeamName
            ViewBag.TeamName = teamName ?? "Home";

            // get the record of bowlers of a certain team
            var record = bowler.Bowlers
                .Include(x => x.Team)
                .Where(x => x.Team.TeamName == teamName || teamName == null)
                .ToList();
            return View(record);
        }

        //get and post methods for the bowler submission form
        [HttpGet]
        public IActionResult BowlerSubmission()
        {
            ViewBag.Teams = bowler.Teams.ToList();

            return View();
        }

        //make sure to add and save changes to the bowlers in the  database
        [HttpPost]
        public IActionResult BowlerSubmission(Bowler b)
        {
            if (ModelState.IsValid)
            {
                bowler.Add(b);
                bowler.SaveChanges();

                return View("Confirmation", b);
            }
            else //validation
            {
                ViewBag.Teams = bowler.Teams.ToList();

                return View();
            }

        }

        //edit record
        [HttpGet]
        public IActionResult Edit(int bowlerid)
        {

            ViewBag.Teams = bowler.Teams.ToList();

            var record = bowler.Bowlers.Single(x => x.BowlerID == bowlerid);

            return View("BowlerSubmission", record);
        }


        [HttpPost]
        public IActionResult Edit(Bowler blah)
        {
            bowler.Update(blah);
            bowler.SaveChanges();

            return RedirectToAction("Index");
        }

        //delete record
        [HttpGet]
        public IActionResult Delete(int bowlerid)
        {
            var blah = bowler.Bowlers.Single(x => x.BowlerID == bowlerid);

            return View(blah);
        }

        [HttpPost]
        public IActionResult Delete(Bowler br)
        {
            
            //blahContext.Bowlers.Remove(br);
            foreach (var tatertot in bowler.Bowlers)
            {
                if(tatertot.BowlerID == br.BowlerID)
                {
                    bowler.Bowlers.Remove(tatertot);
                }
            }
            bowler.SaveChanges();

            return RedirectToAction("Index", new { teamName = "" });
        }

    }

}
