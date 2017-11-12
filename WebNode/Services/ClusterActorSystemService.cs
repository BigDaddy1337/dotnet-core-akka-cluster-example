using System.Threading.Tasks;
using Akka.Actor;
using WebNode.Actors;

namespace WebNode.Services
{
    public interface IClusterActorSystemService
    {
        Task<string> GetAnswerFromCluster(string message);
    }

    public class ClusterActorSystemService : IClusterActorSystemService
    {
        private readonly IActorRef askActor;

        public ClusterActorSystemService(IClusterBuilder clusterBuilder)
        {
            var actorSystem = clusterBuilder.GetClusterNode();
            askActor = actorSystem.ActorOf(Props.Create<ClusterAskActor>(), "ClusterAskActor");
        }

        public Task<string> GetAnswerFromCluster(string message)
        {
            return askActor.Ask<string>(message);
        }
    }
}
