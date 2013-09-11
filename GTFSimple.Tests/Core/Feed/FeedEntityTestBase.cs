using System;
using System.IO;
using System.Text;
using CsvHelper;
using NUnit.Framework;

namespace GTFSimple.Tests.Core.Feed
{
    public class FeedEntityTestBase
    {
        protected static void AssertCsvRow<T>(T entity, string expected) where T : class
        {
            var sb = new StringBuilder();
            using (var csvWriter = new CsvWriter(new StringWriter(sb)))
            {
                csvWriter.WriteRecord(entity);
            }

            Assert.AreEqual(expected + Environment.NewLine, sb.ToString());
        }

        protected static void AssertHeader<T>(string expected) where T : class
        {
            var sb = new StringBuilder();
            using (var csvWriter = new CsvWriter(new StringWriter(sb)))
            {
                csvWriter.WriteHeader<T>();
            }

            Assert.AreEqual(expected + Environment.NewLine, sb.ToString());
        }
    }
}