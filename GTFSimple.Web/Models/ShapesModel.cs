using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GTFSimple.Core.Feed;
using GTFSimple.Kml;
using Newtonsoft.Json;
using SharpKml.Base;

namespace GTFSimple.Web.Models
{
    public class ShapesModel
    {
        private static readonly Repository repo = new Repository();

        public IEnumerable<SelectListItem> Routes
        {
            get
            {
                return from r in repo.GetRoutes()
                       select new SelectListItem
                       {
                           Value = r.Item1,
                           Text = r.Item2,
                           Selected = RouteId == r.Item1,
                       };
            }
        }

        public string RouteId { get; set; }

        public string ShapeId { get; set; }

        public string Segments { get; set; }

        public bool HasShape
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ShapeId)
                       && !string.IsNullOrWhiteSpace(Segments);
            }
        }

        public string GenerateSegmentsJson()
        {
            return JsonConvert.SerializeObject(GenerateSegments());
        }

        public IEnumerable<Shape> GenerateShape()
        {
            var segments = GenerateSegments();

            return segments.SelectMany(s => s.Coordinates)
                           .DistinctUntilChanged()
                           .Select((p, i) => new Shape
                           {
                               Id = ShapeId,
                               PointLatitude = (decimal)p.Latitude,
                               PointLongitude = (decimal)p.Longitude,
                               PointSequence = (uint)i,
                           });
        }

        private IEnumerable<Segment> GenerateSegments()
        {
            var route = repo.GetRoute(RouteId);

            var segments =
                from line in Segments.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                let reverse = line[0] == '-'
                let id = line.Substring(1).Split(' ')[0]
                select new {id, reverse}
                into x
                let rs = route.GetSegment(x.id)
                where rs != null
                select new Segment(rs, x.reverse);
            return segments;
        }
    }

    public class Segment
    {
        private readonly RouteSegment rs;
        private readonly bool reverse;

        public Segment(RouteSegment rs, bool reverse)
        {
            this.rs = rs;
            this.reverse = reverse;
        }

        public string Id { get { return rs.Id; } }

        public string Name { get { return rs.Name; } }

        public IEnumerable<Vector> Coordinates
        {
            get { return reverse ? rs.Coordinates.Reverse() : rs.Coordinates; }
        }
    }
}