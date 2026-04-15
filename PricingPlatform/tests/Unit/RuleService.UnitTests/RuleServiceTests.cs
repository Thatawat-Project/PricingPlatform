//using System;
//using System.Collections.Generic;
//using System.Text;
//using RuleService.Application.DTOs;
//using RuleService.Application.Services;
//using RuleService.UnitTests.Fakes;

//namespace RuleService.UnitTests
//{
//    public class RuleServiceTests
//    {
//        private readonly RuleEngineService _service;

//        public RuleServiceTests()
//        {
//            _service = new RuleEngineService(new FakeEventPublisher());
//        }

//        [Fact]
//        public async Task Create_ShouldReturnId()
//        {
//            var id = await _service.CreateAsync(new CreateRuleRequest
//            {
//                Type = "WeightTier",
//                ConfigJson = "{}",
//                Priority = 1
//            });

//            Assert.NotEqual(Guid.Empty, id);
//        }

//        [Fact]
//        public async Task Publish_ShouldIncreaseVersion()
//        {
//            var id = await _service.CreateAsync(new CreateRuleRequest
//            {
//                Type = "Test",
//                ConfigJson = "{}",
//                Priority = 1
//            });

//            await _service.PublishAsync(id);

//            var active = await _service.GetActiveAsync();
//            Assert.Single(active);
//        }
//    }
//}
