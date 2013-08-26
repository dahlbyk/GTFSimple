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
            var root = XDocument.Load(FilePath("crtransit.kml")).Root;
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
                           let routeMatch =
                               Regex.Match(description, @"<td>Route</td>[^<]+<td>(?<route>[^<]+)</td>",
                                           RegexOptions.Singleline).Groups["route"]
                           let route = routeMatch == null ? "(none)" : routeMatch.Value
                           group new { description, placemarkId, placemarkName, placemark } by route
                           into g
                           orderby g.Key
                           select g;

            foreach (var g in document)
            {
                Console.WriteLine(g.Key);
                foreach (var s in g)
                    Console.WriteLine("  " + s.placemarkId + ": " + s.placemarkName);

                var kml = new XDocument(
                    new XDeclaration("1.0", "UTF-8", "yes"),
                    new XElement(ns + "Document"
                        , new XElement(ns + "name", "Route " + g.Key)
                                 , g.Select(x => x.placemark)
                                 ));
                kml.Save(FilePath(g.Key + ".kml"));
            }

        }

        private static string FilePath(string kml)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, kml);
        }
    }
}
