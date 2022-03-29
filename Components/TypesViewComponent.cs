using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mission13.Models;

namespace Mission13.Components
{
    public class TypesViewComponent : ViewComponent
    {
        // bowlingleaguecontext
        private BowlingLeagueDbContext bowler { get; set; }

        // constructor
        public TypesViewComponent(BowlingLeagueDbContext temp)
        {
            bowler = temp;
        }

        // invoke
        public IViewComponentResult Invoke()
        {
            // route data of teamName --> assign in ViewBag.SelectedTeam
            ViewBag.SelectedTeam = RouteData?.Values["teamName"] ?? "";

            // get team names
            var teams = bowler.Bowlers
                .Select(x => x.Team.TeamName)
                .Distinct()
                .OrderBy(x => x);

            return View(teams);
        }
    }
}

