﻿using ECommerce.Modules.Shared.Customers.RestockSubscriptions.Events.Integration;
using Humanizer;
using MassTransit;
using RabbitMQ.Client;

namespace ECommerce.Modules.Customers.RestockSubscriptions;

internal static class MassTransitExtensions
{
    internal static void AddRestockSubscriptionPublishers(this IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.Message<RestockSubscriptionCreated>(e =>
            e.SetEntityName(
                $"{nameof(RestockSubscriptionCreated).Underscore()}.input_exchange")); // name of the primary exchange
        cfg.Publish<RestockSubscriptionCreated>(e => e.ExchangeType = ExchangeType.Direct); // primary exchange type
        cfg.Send<RestockSubscriptionCreated>(e =>
        {
            // route by message type to binding fanout exchange (exchange to exchange binding)
            e.UseRoutingKeyFormatter(context =>
                context.Message.GetType().Name.Underscore());
        });
    }
}
