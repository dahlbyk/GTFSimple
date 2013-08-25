using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace GTFSimple.Kml
{
    public class Sandbox
    {
        static void Test()
        {
            var kml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "crtransit.kml"));

            //Console.WriteLine(kml);

            var root = XDocument.Parse(kml).Root;
            if (root == null)
                return;

            var ns = (XNamespace) "http://www.opengis.net/kml/2.2";
            var document = from doc in root.Elements(ns + "Document")
                           let docName = (string) doc.Element(ns + "name")
                           from folder in doc.Elements(ns + "Folder")
                           let folderName = (string) folder.Element(ns + "name")
                           from placemark in folder.Elements(ns + "Placemark")
                           let placemarkId = (string)placemark.Attribute("id")
                           let placemarkName = (string)placemark.Element(ns + "name")
                           let description = (string) placemark.Element(ns + "description")
                           let idMatch = 
                               Regex.Match(description, @"<td>ID</td>[^<]+<td>(?<id>[^<]+)</td>",
                                           RegexOptions.Singleline).Groups["id"]
                           let id = idMatch == null ? null : idMatch.Value
                           let streetMatch = 
                               Regex.Match(description, @"<td>(?<street>[^<]+)</td>",
                                           RegexOptions.Singleline).Groups["street"]
                           let street = streetMatch == null ? null : streetMatch.Value
                           let routeMatch =
                               Regex.Match(description, @"<td>Route</td>[^<]+<td>(?<route>[^<]+)</td>",
                                           RegexOptions.Singleline).Groups["route"]
                           let route = routeMatch == null ? "(none)" : routeMatch.Value
                           group new { street, id, description, placemarkId, placemarkName, placemark } by route
                           into g
                           orderby g.Key
                           select g;

            foreach (var g in document)
            {
                Console.WriteLine(g.Key);
                foreach (var s in g)
                    Console.WriteLine("  " + s.placemarkId + ": " + s.placemarkName);
            }

        }
    }
}
