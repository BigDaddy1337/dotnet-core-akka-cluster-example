# Akka.NET Cluster Example

Example shows how to create a simple cluster with Akka.NET on dotnet core.  
Solution based on .NET Core 2.0 and include two seed nodes as console app 

* ClusterSeedNodeOne
* ClusterSeedNodeTwo

and ASP.Core Web API as entry point in cluster
* WebNode

### Starting cluster

First of all we need run both seed nodes

```
dotnet run --project ClusterSeedNodeOne/ClusterSeedNodeOne.csproj
dotnet run --project ClusterSeedNodeTwo/ClusterSeedNodeTwo.csproj
```

then run Web API entry point

```
dotnet run --project WebNode/WebNode.csproj
```

After that we can open in browser url with GET request on /api/testcluster
```
http://localhost:3000/api/testcluster?message=testtextmessage
```
and give simple response, this means that the response was processed in our local cluster.

Also we can send GET request on 
```
http://localhost:3000/api/testactor?message=testtextmessageforactor
```
which will process a simple actor inside WebNode project and called 'LocalSimpleActor'
