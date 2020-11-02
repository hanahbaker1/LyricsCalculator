# Setup Kafka

To setup Kafka with data for a development environment, use the `SetUp.ps1` file within the `DevSetup` folder.

This will first check if Kafka is running via a container name of "kafka" (Verify any other running Kafka containers first), and populate the instance with dummy acommodation data. 

The regions, properties, rooms and other Orlando data are based on SQL queries and supports running searches based on real queries.

# Integrating with Search Properties

## Installation

First off, ensure you install the following nuget package:

`dotnet add package Jet2.Holidays.Search.Properties.ActorSystem`

Next, the project needs to be configured from your Web API startup in a following places:

## Actor System

Assuming your system already has a started actor system, the nuget package provides extension methods to an actor system:

```c#
    var actorSystem = StartActorSystem(services);
    services.AddSearchProperties(Configuration, actorSystem);
    AddActorSystemDependencyResolver(actorSystem, services);
```

Starting an Actor System in the provided API looks like:

```c#
    private Akka.Actor.ActorSystem StartActorSystem(IServiceCollection services)
    {
        var akkaConfig = Configuration.GetSection("akka").Value;
        var actorSystem = Akka.Actor.ActorSystem.Create("[YOURNAME]", akkaConfig);
        
        services.AddSingleton(actorSystem);
        services.AddSingleton(actorSystem.Log);

        return actorSystem;
    }
```

Most of the other config and setup can be seen in the [Startup.cs](http://tfs01.harrogate.jet2.com:8080/tfs/Default/J2H/_git/Search.Properties?path=%2FJet2.Holidays.Search.Properties.WebApi%2FStartup.cs&version=GBdevelop&_a=contents).

## Kafka Setup

The current package relies on data flowing from Kafka, to get this data running currently you need to ensure the following lines are added to startup:

```c#
    services.AddKafka();
    services.AddHostedService<ConsumerHostedService>();
```

# Making a Search

Fortunately, only a single interface is required to perform a search 👍

```c#
    public interface ISearchRootActor
    {
        Task<SearchActor.SearchResultsMessage> Search(SearchActor.SearchRequestMessage query);
    }
```



