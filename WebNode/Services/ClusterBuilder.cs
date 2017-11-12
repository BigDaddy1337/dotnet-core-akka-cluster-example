using Akka.Actor;
using Akka.Cluster;
using Akka.Configuration;

namespace WebNode.Services
{
    public interface IClusterBuilder
    {
        ActorSystem GetClusterNode();
        void StartClusterNode();
        void StopClusterNode();
    }

    public class ClusterBuilder : IClusterBuilder
    {
        private ActorSystem clusterActorSystem;

        public ActorSystem GetClusterNode() => clusterActorSystem;

        public void StartClusterNode()
        {
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
                            port = 0
                        }
					}
					cluster {
						seed-nodes = [
                            ""akka.tcp://ClusterSystem@0.0.0.0:7001"",
                            ""akka.tcp://ClusterSystem@0.0.0.0:7002""
                        ]
                        roles = [client-web-node]
					}
				}
            ");

            clusterActorSystem = ActorSystem.Create("ClusterSystem", config);
        }

        public void StopClusterNode()
        {
            var cluster = Cluster.Get(clusterActorSystem);
            cluster.RegisterOnMemberRemoved(MemberRemoved);
            cluster.Leave(cluster.SelfAddress);
        }

        private async void MemberRemoved()
        {
            await clusterActorSystem.Terminate();
        }
    }
}
