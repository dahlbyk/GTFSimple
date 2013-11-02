using System;
using System.Collections.Generic;
using System.Linq;
using SharpKml.Base;
using SharpKml.Dom;

namespace GTFSimple.Kml
{
    public class RouteSegment
    {
        protected RouteSegment() { }

        public virtual string Id { get; private set; }

        public virtual string Name { get; private set; }

        public virtual IEnumerable<IEnumerable<Vector>> Coordinates { get; private set; }

        public virtual Vector Start
        {
            get
            {
                return Coordinates == null
                           ? null
                           : Coordinates.SelectMany(c => c).FirstOrDefault();
            }
        }

        public virtual Vector End
        {
            get
            {
                return Coordinates == null
                           ? null
                           : Coordinates.SelectMany(c => c).LastOrDefault();
            }
        }

        public virtual string Overlaps(RouteSegment rs, bool end, double epsilon)
        {
            if (Id == rs.Id)
                return null;

            var compare = end ? End : Start;

            var startDelta = Math.Abs((compare - rs.Start).Magnitude);
            var endDelta = Math.Abs((compare - rs.End).Magnitude);

            if (startDelta < endDelta && startDelta < epsilon)
                return "+" + rs.Id + "\t" + rs.Name + "\t" + startDelta;

            if (endDelta < epsilon)
                return "-" + rs.Id + "\t" + rs.Name + "\t" + endDelta;

            return null;
        }

        public static explicit operator RouteSegment(Placemark placemark)
        {
            if (placemark == null)
                return null;

            var geo = (MultipleGeometry)placemark.Geometry;
            return new RouteSegment
            {
                Id = placemark.Id,
                Name = placemark.Name,
                Coordinates = geo.Geometry
                                 .Cast<LineString>()
                                 .Select(line => line.Coordinates)
                                 .ToList(),
            };
        }
    }
}