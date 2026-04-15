using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PricingPlatform.Contracts.Utils
{
    public static class JsonHelper
    {
        // 👇 ใส่ตรงนี้ (ระดับ class)
        private static readonly JsonSerializerOptions DefaultOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        public static string NormalizeJson(string input)
        {
            using var doc = JsonDocument.Parse(input);

            return doc.RootElement.ValueKind == JsonValueKind.String
                ? doc.RootElement.GetString()!
                : doc.RootElement.GetRawText();
        }

        public static T? DeserializeFlexible<T>(string input, JsonSerializerOptions? options = null)
        {
            var json = NormalizeJson(input);

            return JsonSerializer.Deserialize<T>(
                json,
                options ?? DefaultOptions // 👈 ใช้ตรงนี้
            );
        }
    }
}
