using GTFSimple.Core.Feed;
using NUnit.Framework;

namespace GTFSimple.Tests.Core.Feed
{
    [TestFixture]
    public class TripTests : FeedEntityTestBase
    {
        [Test]
        public void HeaderHasExpectedFields()
        {
            AssertHeader<Trip>("");
        }

        [Test]
        public void EmptyTripHasExpectedValues()
        {
            var entity = new Trip();

            AssertCsvRow(entity, "");
        }

        [Test]
        public void PopulatedEntityHasExpectedValues()
        {
            var entity = new Trip
            {
            };

            AssertCsvRow(entity, "");
        }
    }
}