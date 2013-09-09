using System.Collections.Generic;
using System.Linq;
using SharpKml.Dom;

namespace GTFSimple.Kml
{
    public class Route
    {
        protected Route() { }

        public virtual string Id { get; private set; }

        public virtual string Name { get; private set; }

        public virtual IEnumerable<RouteSegment> Segments { get; private set; }

        public static explicit operator Route(Feature feature)
        {
            var folder = feature as Folder;
            if (folder == null)
                return null;

            return new Route
            {
                Id = folder.Id,
                Name = folder.Name,
                Segments = folder.Features
                                 .OfType<Placemark>()
                                 .Select(p => (RouteSegment)p)
                                 .ToList(),
            };
        }
    }
}