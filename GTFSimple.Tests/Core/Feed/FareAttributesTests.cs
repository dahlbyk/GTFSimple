using GTFSimple.Core.Feed;
using NUnit.Framework;

namespace GTFSimple.Tests.Core.Feed
{
    [TestFixture]
    public class FareAttributesTests : FeedEntityTestBase
    {
        [Test]
        public void HeaderHasExpectedFields()
        {
            AssertHeader<Agency>("");
        }

        [Test]
        public void DefaultEntityHasExpectedValues()
        {
            var entity = new Agency();

            AssertCsvRow(entity, "");
        }

        [Test]
        public void PopulatedEntityHasExpectedValues()
        {
            var entity = new Agency
            {
            };

            AssertCsvRow(entity, "");
        }
    }
}