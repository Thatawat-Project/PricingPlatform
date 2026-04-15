using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using RulesService.Application.Interface;
using RulesService.Domain.Entities;

namespace RulesService.Infrastructure.Seed
{
    public static class RuleSeeder
    {
        public static void SeedFromFile(IRuleRepository repo, string filePath)
        {
            if (!File.Exists(filePath))
                return;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    //new System.Text.Json.Serialization.JsonStringEnumConverter()
                }
            };


            var json = File.ReadAllText(filePath);

            var rules = JsonSerializer.Deserialize<List<Rule>>(json, options) ?? new();

            foreach (var rule in rules)
            {
                repo.Add(rule);
            }
        }
    }
}
