using System;
using System.Linq;
using SharpKml.Base;

namespace GTFSimple.Kml
{
    public class Sandbox
    {
        public void Routes()
        {
            var gtc = new Vector(41.9703, -91.66, 0);

            var repo = new Repository();
            foreach (var t in repo.GetRoutes().Take(1))
            {
                var r = repo.GetRoute(t.Item1);
                Console.WriteLine(r.Name);

                var closest =
                    r.Segments.Select(s => new { Id = "+" + s.Id, s.Name, (s.Start - gtc).Magnitude })
                    .Concat(r.Segments.Select(s => new { Id = "-" + s.Id, s.Name, (s.End - gtc).Magnitude }))
                    .OrderBy(x => x.Magnitude)
                    .Take(2)
                    .ToList();

                foreach (var c in closest)
                    Console.WriteLine("\t{1}\t{2}\t{0:f4}", c.Magnitude, c.Id, c.Name);
                Console.WriteLine();

                foreach (var p in r.Segments)
                {
                    Console.WriteLine("\t{0}\t{1}", p.Id, p.Name);
                    Console.WriteLine("\t\t{0} to {1}\t\t{2}", p.Start, p.End, (p.Start - p.End).Magnitude);

                    const double epsilon = 0.0011;
                    var overlaps =
                        from end in new[] { false, true }
                        from n in r.Segments
                        select p.Overlaps(n, end, epsilon)
                            into o
                            where o != null
                            select o;

                    foreach (var o in overlaps.Distinct())
                        Console.WriteLine("\t\t\t" + o);
                }

                Console.WriteLine();
            }
        }
    }
}
