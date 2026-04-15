using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;
using System.Text.Json;
using CsvHelper;
using PricingPlatform.Contracts.DTOs;

namespace PricingService.Infrastructure.Parsers
{
    public static class BulkFileParser
    {
        public static async Task<IReadOnlyList<QuoteRequest>> ParseAsync(
            string fileName,
            Stream stream,
            CancellationToken ct)
        {
            if (!fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                throw new NotSupportedException("Only CSV files are supported");

            return await ParseCsvAsync(stream);
        }

        private static async Task<IReadOnlyList<QuoteRequest>> ParseCsvAsync(Stream stream)
        {
            using var reader = new StreamReader(stream, leaveOpen: true);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = new List<QuoteRequest>();

            await foreach (var record in csv.GetRecordsAsync<QuoteRequest>())
            {
                records.Add(record);
            }

            return records;
        }
    }
}
