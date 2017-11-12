using Akka.Actor;

namespace WebNode.Actors
{
    public class LocalSimpleActor : ReceiveActor
    {
        public LocalSimpleActor()
        {
            Receive<string>(message => GetResponse(message));
        }

        private void GetResponse(string message)
        {
            Sender.Tell($"Сообщение {message} успешно обработано");
        }   
    }
}
