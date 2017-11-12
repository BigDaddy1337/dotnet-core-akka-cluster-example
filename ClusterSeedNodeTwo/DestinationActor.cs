using System;
using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;

namespace ClusterSeedNodeTwo
{
    public class DestinationActor : ReceiveActor
    {
        public DestinationActor()
        {
            // activate the extension
            var mediator = DistributedPubSub.Get(Context.System).Mediator;

            // register to the path
            mediator.Tell(new Put(Self));

            Receive<string>(message => HandleMessage(message));
        }

        private void HandleMessage(string message)
        {
            Console.WriteLine($"Обработка сообщения '{message}'");
            Sender.Tell($"Сообщение '{message}' успешно обработано в кластере, узел ClusterSeedNodeTwo");
        }
    }
}
