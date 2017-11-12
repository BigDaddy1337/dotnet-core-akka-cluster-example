using System.Threading.Tasks;
using Akka.Actor;
using WebNode.Actors;

namespace WebNode.Services
{
    public interface IActorSystemService
    {
        Task<string> GetResponseFromSimpleActor(string message);
    }

    public class ActorSystemService: IActorSystemService
    {
        private readonly IActorRef actor;

        public ActorSystemService(ActorSystem actorSystem)
        {
            actor = actorSystem.ActorOf(Props.Create<LocalSimpleActor>(), "LocalSimpleActor");
        }

        public Task<string> GetResponseFromSimpleActor(string message)
        {
            return actor.Ask<string>(message);
        }
    }
}
