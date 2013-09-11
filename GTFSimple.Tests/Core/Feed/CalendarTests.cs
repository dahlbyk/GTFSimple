using System;
using GTFSimple.Core.Feed;
using NUnit.Framework;

namespace GTFSimple.Tests.Core.Feed
{
    [TestFixture]
    public class CalendarTests : FeedEntityTestBase
    {
        [Test]
        public void HeaderHasExpectedFields()
        {
            AssertHeader<Calendar>("service_id,monday,tuesday,wednesday,thursday,friday,saturday,sunday,start_date,end_date");
        }

        [Test]
        public void DefaultEntityHasExpectedValues()
        {
            var entity = new Calendar();

            AssertCsvRow(entity, ",0,0,0,0,0,0,0,00010101,00010101");
        }

        [Test]
        public void PopulatedEntityHasExpectedValues()
        {
            var entity = new Calendar
            {
                ServiceId = "WD",
                Monday = true,
                Tuesday = false,
                Wednesday = true,
                Thursday = false,
                Friday = true,
                Saturday = false,
                Sunday = false,
                StartDate = new DateTime(2007, 9, 8),
                EndDate = new DateTime(2011, 7, 10),
            };

            AssertCsvRow(entity, "WD,1,0,1,0,1,0,0,20070908,20110710");
        }
    }
}
