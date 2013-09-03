using System;
using System.IO;
using System.Linq;
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

            var ns = (XNamespace)"http://www.opengis.net/kml/2.2";
            var document = from doc in root.Elements(ns + "Document")
                           from folder in doc.Elements(ns + "Folder")
                           from placemark in folder.Elements(ns + "Placemark")
                           select new Placemark(placemark) into placemark
                           group placemark by placemark.Route
                               into g
                               orderby g.Key
                               select g;

            foreach (var g in document)
            {
                Console.WriteLine(g.Key);
                foreach (var s in g)
                    Console.WriteLine("  " + s.Id + ": " + s.Name);

            }

            var kml = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement(
                    ns + "Document"
                    , new XElement(ns + "name", "Transit_Routes")
                    , document.Select(g =>
                                      new XElement(
                                          ns + "Folder"
                                          , new XElement(ns + "name", "Route " + g.Key)
                                          , g.Select(x => (XElement)x)
                                          ))));
            var filePath = FilePath("ByRoute.kml");
            Console.WriteLine(filePath);
            kml.Save(filePath);
        }

        private static string FilePath(string kml)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, kml);
        }
    }
}
