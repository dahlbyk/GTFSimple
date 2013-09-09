using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SharpKml.Base;
using SharpKml.Dom;

namespace GTFSimple.Kml
{
    public class Repository
    {
        private static readonly Container document = ParseKml();

        private static Container ParseKml()
        {
            var parser = new Parser();
            using (var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "crtransit.kml")))
                parser.Parse(sr);

            var root = parser.Root as Container;
            if (root == null)
                throw new IOException(string.Format("Could not load KML file. Expected Container; found {0}.",
                                                    parser.Root == null ? "(null" : parser.GetType().Name));

            return root;
        }

        public IEnumerable<Tuple<string, string>> GetRoutes()
        {
            return from folder in document.Features.OfType<Folder>()
                   orderby folder.Name
                   select Tuple.Create(folder.Id, folder.Name);
        }

        public Route GetRoute(string id)
        {
            return (Route)document.FindFeature(id);
        }
    }
}
