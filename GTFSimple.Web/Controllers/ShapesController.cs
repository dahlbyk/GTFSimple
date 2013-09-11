using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
using GTFSimple.Core.Feed;
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

        public ActionResult Index(ShapesModel data, string csv)
        {
            var model = data ?? new ShapesModel();

            model.RouteId = model.RouteId ?? "R01";
            model.ShapeId = model.ShapeId ?? "S01";
            model.Segments = model.Segments ?? defaultSegments;

            if (!string.IsNullOrEmpty(csv))
                return Csv(model);

            return View(model);
        }

        private ActionResult Csv(ShapesModel model)
        {
            return new CsvResult<Shape>(model.GenerateShape(), model.ShapeId);
        }
    }

    internal class CsvResult<T> : FileResult where T : class
    {
        private readonly IEnumerable<T> rows;
        private readonly string attachmentFilename;

        public CsvResult(IEnumerable<T> rows, string attachmentFilename)
            : base("text/csv")
        {
            this.rows = rows;
            this.attachmentFilename = attachmentFilename;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            if(!string.IsNullOrEmpty(attachmentFilename))
                response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.csv", attachmentFilename));

            using (var csvWriter = new CsvWriter(response.Output))
                csvWriter.WriteRecords(rows);
        }
    }
}