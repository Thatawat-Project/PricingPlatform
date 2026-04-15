//using System;
//using System.Collections.Generic;
//using System.Text;
//using RuleService.Application.Interfaces;

//namespace RuleService.UnitTests.Fakes
//{
//    public class FakeEventPublisher : IEventPublisher
//    {
//        public List<(string Event, object Payload)> Events { get; } = new();

//        public Task PublishAsync(string eventName, object payload)
//        {
//            Events.Add((eventName, payload));
//            return Task.CompletedTask;
//        }
//    }
//}
