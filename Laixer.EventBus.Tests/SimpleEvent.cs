using Laixer.EventBus.Handler;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Laixer.EventBus.Tests
{
    public class SimpleEvent
    {
        private interface ISimpleEvent : IEvent
        {
            int Counter { get; set; }
        }

        private class SimpleEventImpl : ISimpleEvent
        {
            public int Counter { get; set; } = 0;
        }

        private class SimpleEventHandler : IEventHandler<ISimpleEvent>
        {
            public Task HandleEventAsync(EventHandlerContext<ISimpleEvent> context, CancellationToken cancellationToken = default)
            {
                context.Event.Counter++;

                return Task.CompletedTask;
            }
        }

        private IEventBusBuilder BuildEventBusService(IServiceCollection services)
        {
            return services.AddEventBus();
        }

        private EventBusService GetEventBusService(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            return sp.GetRequiredService<EventBusService>();
        }

        [Fact]
        public async Task CreateEventBusAndFire()
        {
            // Arrange
            var col = new ServiceCollection();
            BuildEventBusService(col);

            var eventBus = GetEventBusService(col);

            // Act
            await eventBus.FireEventAsync(new SimpleEventImpl());
        }

        [Fact]
        public async Task TriggerBasicEventHandler()
        {
            // Arrange
            var col = new ServiceCollection();
            var builder = BuildEventBusService(col);

            builder.AddHandler<ISimpleEvent, SimpleEventHandler>();

            var eventBus = GetEventBusService(col);

            // Act
            var myEvent = new SimpleEventImpl();
            await eventBus.FireEventAsync(myEvent);

            // Assert
            Assert.Equal(1, myEvent.Counter);
        }

        [Fact]
        public async Task CancelOperationBeforeFiredEvent()
        {
            // Arrange
            var col = new ServiceCollection();
            var builder = BuildEventBusService(col);

            builder.AddHandler<ISimpleEvent, SimpleEventHandler>();

            var eventBus = GetEventBusService(col);
            var myEvent = new SimpleEventImpl
            {
                Counter = 901
            };
            using var cts = new CancellationTokenSource();

            // Act
            cts.Cancel();

            // Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() => eventBus.FireEventAsync(myEvent, cts.Token));
        }
    }
}
