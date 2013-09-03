using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GTFSimple.Core;

namespace GTFSimple.Web.Models
{
    public class StopTimesModel
    {
        private static readonly Regex whitespace = new Regex(@"[^\S]+", RegexOptions.Compiled);
        private static readonly Regex time = new Regex(@"(?<h>\d+):(?<m>\d+)", RegexOptions.Compiled);

        public string Stops { get; set; }
        public string Trips { get; set; }
        public string TripPrefix { get; set; }

        public bool HasStopTimes
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Stops)
                       && !string.IsNullOrWhiteSpace(Trips);
            }
        }

        public IEnumerable<StopTime> GenerateStopTimes()
        {
            if (Stops == null || Trips == null)
                yield break;

            var stops =
                (
                    from line in Stops.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    let split = whitespace.Split(line)
                    let stopId = split[0]
                    let offsetMinutes = split.Length > 1 ? split[1].ConvertToInt32() : default(int?)
                    let offset =
                        offsetMinutes == null ? default(TimeSpan?) : TimeSpan.FromMinutes(offsetMinutes.Value)
                    select new { stopId, offset }
                ).ToList();

            var trips =
                (
                    from line in Trips.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    let split = whitespace.Split(line)
                    let tripId = split[0]
                    let startTime = split.Length > 1 ? split[1].ConvertToTimeSpan() : default(TimeSpan?)
                    select new { tripId, startTime }
                ).ToList();

            foreach (var trip in trips)
                foreach (var stop in stops)
                {
                    yield return new StopTime
                    {
                        TripId = TripPrefix + trip.tripId,
                        ArrivalTime = trip.startTime + stop.offset,
                        DepartureTime = trip.startTime + stop.offset,
                        StopId = stop.stopId,
                    };
                }
        }

        private static TimeSpan? ParseTime(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            var match = time.Match(s);
            return match.Success
                       ? new TimeSpan(match.Groups["h"].Value.ConvertToInt32() ?? 0,
                                      match.Groups["m"].Value.ConvertToInt32() ?? 0, 0)
                       : default(TimeSpan?);
        }
    }
}