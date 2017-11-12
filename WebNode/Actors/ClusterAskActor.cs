using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;

namespace WebNode.Actors
{
    public class ClusterAskActor : ReceiveActor
    {
        private readonly IActorRef mediator;

        public ClusterAskActor()
        {
            // activate the extension
            mediator = DistributedPubSub.Get(Context.System).Mediator;
            Receive<string>(message => GetResponseFromClusterMembers(message));
        }

        private async void GetResponseFromClusterMembers(string message)
        {
            Sender.Tell(await mediator.Ask<string>(new Send("/user/destination", message, localAffinity: true)));
        }
    }
}
