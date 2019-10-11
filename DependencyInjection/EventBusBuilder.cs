using Laixer.EventBus.Internal;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal class EventBusBuilder : IEventBusBuilder
    {
        public IServiceCollection Services { get; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>.</param>
        public EventBusBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Add event hander registration to event bus.
        /// </summary>
        /// <param name="registration"><see cref="EventHandlerRegistration"/>.</param>
        /// <returns><see cref="IEventBusBuilder"/>.</returns>
        public IEventBusBuilder Add(EventHandlerRegistration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            Services.Configure<EventBusServiceOptions>(options =>
            {
                options.Registrations.Add(registration);
            });

            return this;
        }
    }
}
