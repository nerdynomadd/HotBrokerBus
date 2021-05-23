<!-- PROJECT LOGO -->
<h3 align="center">HotBrokerBus</h3>
<p align="center">
    <br />
    <br />
    <a href="https://github.com/Kakktuss/HotMessageBus/issues">Report Bug</a>
    ·
    <a href="https://github.com/Kakktuss/HotMessageBus/issues">Request Feature</a>
</p>


<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#installing">Installing</a></li>
      </ul>
    </li>
    <li>
    <a href="#usage">Usage</a>
      <ul>
        <li><a href="#nats-streaming">Nats streaming</a></li>
        <li>
            <a href="#middlewares">Middlewares</a>
            <ul>
                <li><a href="#middleware-concept">Concept</a></li>
                <li><a href="#middleware-priority">Priority system</a></li>
                <li><a href="#stan-related-middlewares">Default stan related middleware</a></li>
            </ul>
        </li>
      </ul>
    </li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>

<!--ABOUT THE PROJECT-->
## About the project

This project has been built on an idea proposed by microsoft and implemented on their famous [eShopOnContainer project]("https://github.com/dotnet-architecture/eShopOnContainers").
<br/>
It gave me the idea to implement an Integration Event pattern and Integration Command pattern built on top of message brokers to interface them using a simple and easy to use system.

Actually, the project works with these brokers:
* [Nats]("https://docs.nats.io/developing-with-nats/developer")
* [Nats streaming]("https://docs.nats.io/nats-streaming-concepts/intro")

Multiple updates to work with other brokers such as Kafka, RabbitMq, ZeroMQ, etc.. are scheduled but PRs are open if needed :)

<!--GETTING STARTED-->
## Getting Started

### Installing

To install this library just reference it using nuget package manager:

````
dotnet add package HotBrokerBus
````

If this package is being used on an Asp.Net core project you'll need to reference then the package ``Autofac.AspNetCore`` which add a new the support of a new method on your Startup class: ConfigureContainer

<!-- USAGE -->
## Usage

This is an example of how you may setup the library:

Firstly, you may note that this library only work, actually with Autofac modules

On the ``ConfigureContainer`` method provided by the Autofac package you'll be able to register some autofac modules and especially those used by the lib to work:
* StanRegistersAutofacModule: This module registers the entire pipeline classes for both Nats streaming related EventRegister and Nats streaming related CommandRegister modules.

### Nats streaming

````c#
// Get the base StanOptions configuration given by Nats streaming C# client
var options = StanOptions.GetDefaultOptions()

// Configure base nats URL
options.NatsURL = "nats://localhost:4222"

builder.RegisterModule(new StanRegisterAutofacModule("clusterName", "applicationName", options))
````

Here is an example on how to register an IntegrationEvent

```c#
builder.RegisterCallback(container => 
{
    // Retrieve the Stan bus event register
    var eventBus = container.Resolve<IStanBusEventRegister>();
    
    // Get the base StanSubscriptionInfos configuration given by Nats streaming c# client
    var options = StanSubscriptionOptions.GetDefaultOptions();
    options.DeliverAllAvailable();
    
    eventBus.Subscribe<TestEvent, TestEventHandler>("test", "testEvent", "queue_group", options);
});

```

And here is an example on how to register an IntegrationCommand

````c#
builder.RegisterCallback(container => 
{
    // Retrieve the Stan bus command register
    var eventBus = container.Resolve<IStanBusCommandRegister>();
    
    eventBus.Subscribe<TestCommand, TestCommandHandler>("test", "testEvent");
});
````

### Middlewares

#### Middleware concept

Once subscribed, when a new event/command is requested over Nats streaming bus, the message will pass all over the middleware pipeline and then to your EventHandler.

The middleware pipeline was designed to be extendable. 
Saying this, you have to possibility to add one or multiple middlewares to both event/command pipeline at the beginning, middle or end of the pipeline.

To declare a new middleware you'll need to create a new class that extends either IEventBusMiddleware or ICommandBusMiddleware

Here is how you can declare a new event middleware:

````c#
public class MiddleExecutionEventMiddleware : IEventBusMiddleware
{
    public async Task Invoke(BusMiddlewareExecutionDelegate next, IEventExecutionContext context)
    {
        Console.WriteLine("Passes by the middle execution event middleware");
        
        await next(context);
    }
}
````

And then how you can declare a new command middleware:

````c#
public class MiddleExecutionCommandMiddleware : ICommandBusMiddleware
{
    public async Task Invoke(BusMiddlewareExecutionDelegate next, ICommandExecutionContext context)
    {
        Console.WriteLine("Passes by the middle execution command middleware");
        
        await next(context);
    }
}
````

Once declared, you will need to add it on the MiddlewareStorage related to your broker, for STAN it's IStanBusMiddlewareStorage.

/!\ Middlewares should be added **only** on the RegisterCallback method of Autofac library
`````c#
var middlewareStorage = container.Resolve<IStanBusMiddlewareStorage>();

// Here register STAN event middleware
// The parameter middlewarePriority described below is optional as it is by default on BusMiddlewarePriority.Basic but can be changed to BusMiddlewarePriority.First or BusMiddlewarePriority.Last
middlewareStorage.AddEventMiddleware<MiddleExecutionEventMiddleware>(BusMiddlewarePriority.Basic);

// Here register STAN command middleware 
// The parameter middlewarePriority described below is optional as it is by default on BusMiddlewarePriority.Basic but can be changed to BusMiddlewarePriority.First or BusMiddlewarePriority.Last
middlewareStorage.AddCommandMiddleware<MiddleExecutionCommandMiddleware>(BusMiddlewarePriority.Basic);
`````

Moreover, if needed, the middleware registrations can be registered using interfaces. Only if interfaces are linked to their respective implementations using a DI registration method

`````c#
var middlewareStorage = container.Resolve<IStanBusMiddlewareStorage>();

// Here register STAN event middleware using an interface - can only work if the implementation has been registered as a Singleton/Scoped/Transient instance on DI
middlewareStorage.AddEventMiddleware<IMiddleExecutionEventMiddleware>();
`````

#### Middleware priority

Middlewares works on a basic system of priority:
* First: The middleware will be the first one to be executed, useful when you want to add some event/command parsing logic
* Basic: The middleware will be executed between first and last middlewares, this is the default option to added middlewares
* Last: The middleware will be executed after first and basic one are executed, useful when you want to add some class call logic

There's two default middlewares registered on both event and command registers. These are both configured on priority first and last because they are the basic logic of event/command parsing logic and event/command handlers call logic.
These middlewares can be extended by implementing a new class extending their interface:

#### Stan related middlewares
* IStanBusEventParserMiddleware: Handles the parsing logic of events
* IStanBusEventExecutionMiddleware: Handles the execution logic of events
* IStanBusCommandParserMiddleware: Handles the parsing logic of commands
* IStanBusCommandExecutionMiddleware: Handles the execution logic of commands

````c#
public class CustomStanBusEventParserMiddleware : IStanBusEventParserMiddleware
{
    public async Task Invoke(BusMiddlewareExecutionDelegate next, ICommandExecutionContext context)
    {
        // context.Event and context.EventHandler are at this point of the execution null
        // This middleware thus helps to resolve them
        
        await next(context);
    }
}
````

<!-- ROADMAP -->
## Roadmap

Actually there is no roadmap really defined, i will give some updates occasionally to add other broker support and to add some unit tests.

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes depending on [Conventional Commit]("https://www.conventionalcommits.org/en/v1.0.0/") spec (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request


<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.

<!-- CONTACT -->
## Contact

Benjamin Mandervelde - [@kakktuss](https://twitter.com/Kakktuss) - benjaminmanderveldepro@gmail.com
