using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace GTFSimple.Kml
{
    internal class Placemark
    {
        private static readonly Regex routePattern = new Regex(@"<td>Route</td>[^<]+<td>(?<route>[^<]+)</td>", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly XNamespace ns = "http://www.opengis.net/kml/2.2";

        private readonly XElement element;

        public Placemark(XElement element)
        {
            this.element = element;

            Id = (string)element.Attribute("id");
            Name = (string)element.Element(ns + "name");
            Description = (string)element.Element(ns + "description");

            var routeMatch = routePattern.Match(Description).Groups["route"];
            Route = routeMatch == null ? "(none)" : routeMatch.Value;
        }

        public string Description { get; private set; }

        public string Id { get; private set; }

        public string Name { get; private set; }

        public string Route { get; private set; }

        public static implicit operator XElement(Placemark placemark)
        {
            return placemark == null ? null : placemark.element;
        }
    }
}