using System;
using Akka.Actor;
using Akka.Cluster;
using Akka.Configuration;

namespace ClusterSeedNodeOne
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Title = "ClusterSeedNodeOne";

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
                            port = 7001
                        }
					}
					cluster {
						seed-nodes = [
                            ""akka.tcp://ClusterSystem@0.0.0.0:7001"",
                            ""akka.tcp://ClusterSystem@0.0.0.0:7002""
                        ]
                        roles = [seed-node-1]
                    }
				}
            ");

            var actorSystem = ActorSystem.Create("ClusterSystem", config);
            var cluster = Cluster.Get(actorSystem);

            Console.WriteLine("SEED NODE: Actor system created");

            actorSystem.ActorOf(Props.Create<DestinationActor>(), "destination");

            Console.ReadLine();

            cluster.Leave(cluster.SelfAddress);
        }
    }
}
