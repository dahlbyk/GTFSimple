using System.Web.Mvc;
using GTFSimple.Kml;
using GTFSimple.Web.Models;

namespace GTFSimple.Web.Controllers
{
    public class ShapesController : Controller
    {
        private static readonly string defaultSegments = @"
-ID_00020
+ID_00016
+ID_00019
+ID_00018
+ID_00017
+ID_00001
+ID_00351
+ID_00015
-ID_00000
-ID_00021
-ID_00017
-ID_00018
-ID_00019
-ID_00016
+ID_00020
".TrimStart();

        public ActionResult Index(ShapesModel data)
        {
            var model = data ?? new ShapesModel();

            model.RouteId = model.RouteId ?? "R01";
            model.ShapeId = model.ShapeId ?? "S01";
            model.Segments = model.Segments ?? defaultSegments;

            return View(model);
        }
    }
}