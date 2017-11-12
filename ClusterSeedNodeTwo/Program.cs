using System;
using Akka.Actor;
using Akka.Cluster;
using Akka.Configuration;

namespace ClusterSeedNodeTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ClusterSeedNodeTwo";

            var config = ConfigurationFactory.ParseString(@"
				akka {
					actor {
						provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
						serializers {
                            akka-pubsub = ""Akka.Cluster.Tools.PublishSubscribe.Serialization.DistributedPubSubMessageSerializer, Akka.Cluster.Tools""
                        }
                        serialization-bindings {
                            ""Akka.Cluster.Tools.PublishSubscribe.IDistributedPubSubMessage, Akka.Cluster.Tools"" = akka-pubsub
                            ""Akka.Cluster.Tools.PublishSubscribe.Internal.SendToOneSubscriber, Akka.Cluster.Tools"" = akka-pubsub
                        }
                        serialization-identifiers {
                            ""Akka.Cluster.Tools.PublishSubscribe.Serialization.DistributedPubSubMessageSerializer, Akka.Cluster.Tools"" = 9
                        }
					}
					remote {
						log-remote-lifecycle-events = on
						helios.tcp {
							transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
                            applied-adapters = []
                            transport-protocol = tcp
                            hostname = ""0.0.0.0""
                            port = 7002
                        }
					}
					cluster {
						seed-nodes = [
                            ""akka.tcp://ClusterSystem@0.0.0.0:7001"",
                            ""akka.tcp://ClusterSystem@0.0.0.0:7002""
                        ]
                        roles = [seed-node-2]
					}
				}
            ");

            var nodeOneActorSystem = ActorSystem.Create("ClusterSystem", config);
            var cluster = Cluster.Get(nodeOneActorSystem);
            Console.WriteLine("NodeOne: Actor system created");
            nodeOneActorSystem.ActorOf(Props.Create<DestinationActor>(), "destination");

            Console.ReadLine();

            cluster.Leave(cluster.SelfAddress);
        }
    }
}
