using System.Web.Mvc;
using GTFSimple.Web.Models;

namespace GTFSimple.Web.Controllers
{
    public class StopTimesController : Controller
    {
        private static readonly string defaultStops = @"
101	0
102
103
104
105	4
106
107
108
109
110
111	9
112
113
114
115	18
116
117
118
119
120	25
".TrimStart();

        private static readonly string defaultTrips = @"
1 08:00
2 08:35
3 09:10
4 09:25
".TrimStart();

        public ActionResult Index(StopTimesModel data)
        {
            var model = data ?? new StopTimesModel();

            model.Stops = model.Stops ?? defaultStops;
            model.Trips = model.Trips ?? defaultTrips;

            return View(model);
        }

    }
}