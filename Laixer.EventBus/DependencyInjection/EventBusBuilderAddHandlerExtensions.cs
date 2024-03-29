﻿using Laixer.EventBus;
using Laixer.EventBus.Internal;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides basic extension methods for registering <see cref="IEventHandler"/> instances in an <see cref="IEventBusBuilder"/>.
    /// </summary>
    public static class EventBusBuilderAddHandlerExtensions
    {
        /// <summary>
        /// Register event handler for the event interface.
        /// </summary>
        /// <typeparam name="TEventInterface">Event interface of the type <see cref="IEvent"/>.</typeparam>
        /// <typeparam name="TImplementation">Handler for the event interface.</typeparam>
        /// <param name="builder">Instance of <see cref="IEventBusBuilder"/>.</param>
        /// <returns>Instance of <see cref="IEventBusBuilder"/>.</returns>
        public static IEventBusBuilder AddHandler<TEventInterface, TImplementation>(this IEventBusBuilder builder)
            where TEventInterface : IEvent
            where TImplementation : class, IEventHandler<TEventInterface>
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.Add(EventHandlerRegistration.Register<TEventInterface, TImplementation>());
        }
    }
}
