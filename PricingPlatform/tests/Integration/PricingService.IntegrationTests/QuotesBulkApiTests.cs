using System.Net.Http.Headers;
using System.Net.Http.Json;
using PricingService.IntegrationTests.Factories;
using PricingService.IntegrationTests.Models.DTOs;

namespace PricingService.IntegrationTests
{
    public class QuotesBulkApiTests : IClassFixture<TestFactory>
    {
        private readonly HttpClient _client;

        public QuotesBulkApiTests(TestFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Bulk_Should_Create_Job()
        {
            using var content = new MultipartFormDataContent();

            await using var stream = LoadFile("bulk_quotes.csv");

            var file = new StreamContent(stream);
            file.Headers.ContentType = new MediaTypeHeaderValue("text/csv");

            content.Add(file, "file", "bulk.csv");

            var response = await _client.PostAsync("/quotes/bulk", content);

            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadFromJsonAsync<BulkResponse>();

            Assert.NotNull(body);
            Assert.False(string.IsNullOrEmpty(body!.JobId));
        }

        private static Stream LoadFile(string fileName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Data", fileName);

            if (!File.Exists(path))
                throw new FileNotFoundException($"Test file not found: {path}");

            return File.OpenRead(path);
        }
    }
}
