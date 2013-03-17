# FluentSiteMap

A site map solution for MVC that uses a fluent configuration API.

FluentSiteMap provides the following features:
* A rich, highly-testable, hierarchical site map data structure representing the navigable pages of your web site.  It's similar to, but not as complex as classic ASP.NET SiteMaps.
* A fluent API for configuring your site map that can use lambda expressions to load data within the various nodes.
* Out-of-the box partial views that can be used to easily render page titles, menus, bread crumbs, and full site maps from the site map data.
* The ability to filter site map nodes based on the current user's authentication/authorization status.
* The ability to hide site map nodes from menus or bread crumbs.
* A pluggable architecture so you can easily add your own custom filters and ways of loading node data.
* A set of helper classes for unit testing your site map.

FluentSiteMap won't:
* Read your existing MVC controllers and actions to build its structure. You specify that structure explicitly through its fluent configuration API.
* Do your dishes or organize your vinyl collection.

## Getting Started

### Web Project

Typically, you're using FluentSiteMap in a web application project in Visual Studio.  Here's how to get started:

Run the following command in the [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console) to install the [FluentSiteMap](http://nuget.org/packages/TS.FluentSiteMap) NuGet package and configure your web project to use it: 

```
PM> Install-Package TS.FluentSiteMap
```

The NuGet package install performs the following tasks:
* Adds the necessary FluentSiteMap assembly references to your project
* Adds a set of partial views to your `Views/Shared` project directory that you can customize for your site (but work pretty good out of the box)
* Generates a starter site map class in your `Models` directory and wires it up so it builds on application start via a [WebActivator](https://github.com/davidebbo/WebActivator) class placed in your `App_Start` directory

Now you can start configuring your site map:
* Optionally rename the new site map class in your `Models` directory, which by default is called `MySiteMap.cs`.
* Edit your site map class so that it matches your site's actual structure (see the [Fluent API Reference](#fluent-api-reference) for more info).
* If your site map needs to use a dependency (ex: a data repository to load product data), add the dependency to the site map class's constructor and then inject it via the `FluentSiteMapCreator` class in the `App_Start/FluentSiteMapCreator.cs` file, which is where your site map instance is actually created and registered with FluentSiteMap.

The final goal is to actually render content using your site map data.  That's easily done using a set of HTML helpers provided with FluentSiteMap that use the partial views included in the package:

```
<!-- render the current page's title -->
<% Html.FluentSiteMap().Title(); %>

<!-- render a menu -->
<% Html.FluentSiteMap().Menu(); %>

<!-- render bread crumbs -->
<% Html.FluentSiteMap().BreadCrumbs(); %>

<!-- render a full site map -->
<% Html.FluentSiteMap().SiteMap(); %>
```

### Test Project

FluentSiteMap was designed with testability in mind.  It comes with a set of helper classes for testing site maps under a variety of scenarios (ex: simulating users with specific authorization rules) as well as testing your own custom filters.  Currently these helpers work best when using [NUnit](http://nunit.org) as your test running framework and [Rhino Mocks](http://www.hibernatingrhinos.com/oss/rhino-mocks) as your mocking engine.

Run the following command in the [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console) to install the [FluentSiteMap Testing](http://nuget.org/packages/TS.FluentSiteMap.Testing) NuGet package in your test project (which could also be your web project): 

```
PM> Install-Package TS.FluentSiteMap.Testing
```

See the [Testing Reference](#testing-reference) for more information on how to use the testing helpers.

## Under The Hood

### Node Builders

At its core FlentSiteMap uses the [decorator pattern](http://en.wikipedia.org/wiki/Decorator_pattern) to dynamically construct a site map from its configuration.  It does this using a series of **Node Builder** objects (which all implement the [INodeBuilder](/src/FluentSiteMap.Core/INodeBuilder.cs) interface), where each builder wraps its predecessor.  When its time to build the site map itself, a build operation is called on the top-most builder, which gets cascaded down to all of the other builders.  This top-most builder is actually your site map class, which inherits from the main [SiteMap](/src/FluentSiteMap.Core/SiteMap.cs) base class.  To make the work of chaining Node Builder objects together easy, FluentSiteMap comes with a set of [extension methods](#fluent-api-reference) which all operate on a `INodeBuilder` object and return a new one.  This combination of chained `INodeBuilder` objects and the extension methods is what provides FluentSiteMap its "fluent" interface.  The output of a site map build is a heirarchy of [Node](/src/FluentSiteMap.Core/Node.cs) DTO objects, which represent the entire site map structure of your website.

### Node Filters

Building the underlying site map structure is typically a one-time operation when your site map is first accessed by one of your views or controllers.  However, often times you don't want the entire site map to be accessible to a user on a particular page request.  Rather, you want to filter that structure so it's appropriate to the context (ex: only display the Admin nodes of your site map to admin users).  FluentSiteMap uses **Node Filter** objects to accomplish this, which all implement the [INodeFilter](/src/FluentSiteMap.Core/INodeFilter.cs) interface.  Like Node Builders, Node Filters can be easily applied to a node in a site map using the FluentSiteMap [extension methods](#fluent-api-reference).  Through some abstraction provided by a few other core classes (discussed below), consumers of the site map only interact with the final filtered representation of the site map in the form of [FilteredNode](/src/FluentSiteMap.Core/FilteredNode.cs) DTO instances, instead of the raw `Node` objects.

### Coordination

Node Builders and Filters are all coordinated together using a set of core classes:
* [SiteMapCoordinator](/src/FluentSiteMap.Core/SiteMapCoordinator.cs) - This is the main coordinator class that handles the execution of building a site map and applying filtering on the nodes
* [SiteMapHelper](/src/FluentSiteMap.Core/SiteMapHelper.cs) - This is actually the main entry point into the site map data for consumers.  It provides a facade on top of a `SiteMapCoordinator` via static members, which makes for easy access from ASP.NET consumers.

### Extensibility

FluentSiteMap can be extended in three primary ways:

#### Customizing Partial Views

The partial views that get installed with the FluentSiteMap NuGet package (into the `Views/Shared` directory) can be customized to better align with the design and layout of your site.  They use fairly generic HTML mark-up and styling so the need for customization is hopefully minimal.

#### Custom Node Filters

You may wish to add new filtering capabilities to FluentSiteMap.  Out of the box FluentSiteMap allows filtering based on authentication and security role status.  However, your site may have more complex needs.  To create and use a new filter, do the following:
* Create a class that implements the `INodeFilter` interface. See the [TS.FluentSiteMap.Filters](/src/FluentSiteMap.Core/Filters) namespace for examples of Node Filter classes.
* Create an extension method that creates your filter and adds it to a `INodeBuilder` object, making it easy to use your filter in the fluent configuration API.  See the [FilterExtensions](/src/FluentSiteMap.Core/Filters/FilterExtensions.cs) class for examples of these extension methods.

#### Custom Node Builders

If you need to store custom data in your site map and/or one of your custom node filters uses that data for its filtering criteria, you can create a custom Node Builder to accomplish that.  Similar to creating a custom Node Filter, here's how to create a custom Node Builder:
* Create a class that inherits from the [DecoratingNodeBuilder](/src/FluentSiteMap.Core/DecoratingNodeBuilder.cs) base class, which is easier than just implementing the `INodeBuilder` interface.  See the [TS.FluentSiteMap.Builders](/src/FluentSiteMap.Core/Builders) namespace for examples of Node Builder classes.
* Create an extension method that creates your builder using the previous builder, making it easy to use your filter in the fluent configuration API.  See the [BuilderExtensions](/src/FluentSiteMap.Core/Builders/BuilderExtensions.cs) class for examples of these extension methods.
* To test your custom builder, use the `DecoratingNodeBuilderTestHelper` described in more detail in the [Testing Reference](#testing-reference) below.

## Fluent API Reference
FluentSiteMap gets its name from the fluent-style API it uses for configuring its site maps.  This API is essentially a set of extension methods whose calls can be chained to make a very human-readable site map structure.  Internally they use a decorator pattern to build a sequence of "builder" objects which in the end get executed in turn to build the final site map node structure.

When you install the FluentSiteMap package in your Visual Studio web project, it generates a very basic  site map class with only a single node at the root.  Here's an example of a [more complete site map](/src/FluentSiteMap.Sample/Models/MySiteMap.cs), taken from the sample project contained within the FluentSiteMap source code repo which demonstrates the fluent-like API:

```cs
using TS.FluentSiteMap.Builders;
using TS.FluentSiteMap.Filters;

namespace TS.FluentSiteMap.Sample.Models
{
    public class MySiteMap
        : SiteMap
    {
        public MySiteMap(IProductRepository productRepository)
        {
            Root =
                Node()
                    .WithTitle("Home")
                    .WithDescription("Welcome to Foo.com!")
                    .ForController("Home").ForAction("Index").WithUrlFromMvc()
                    .WithChildren(
                        Node()
                            .WithTitle("About Us").WithDescriptionSameAsTitle()
                            .ForAction("About").WithUrlFromMvc(),
                        Node()
                            .WithTitle("Account")
                            .ForController("Account")
                            .HiddenInMenu()
                            .HiddenInBreadCrumbs()
                            .WithChildren(
                                Node()
                                    .WithTitle("Sign In").WithDescriptionSameAsTitle()
                                    .ForAction("LogOn").WithUrlFromMvc()
                                    .IfNotAuthenticated(),
                                Node()
                                    .WithTitle("Sign Out").WithDescriptionSameAsTitle()
                                    .ForAction("LogOff").WithUrlFromMvc()
                                    .IfAuthenticated(),
                                Node()
                                    .WithTitle("Register").WithDescriptionSameAsTitle()
                                    .ForAction("Register").WithUrlFromMvc()),
                        Node()
                            .WithTitle("Products").WithDescriptionSameAsTitle()
                            .ForController("Products").ForAction("Index").WithUrlFromMvc()
                            .WithChildren(productRepository.FetchProducts(),
                                          (p, b) => b
                                                        .WithTitle(p.Name)
                                                        .WithDescription(p.Description)
                                                        .ForAction("View").WithUrlFromMvc(new {id = p.Id})
                                                        .WithMetadata("Price", p.Price)),
                        Node()
                            .WithTitle("Site Map").WithDescriptionSameAsTitle()
                            .ForAction("SiteMap").WithUrlFromMvc(),
                        Node()
                            .WithTitle("Google").WithDescriptionSameAsTitle()
                            .WithUrl("http://google.com")
                            .WithTargetThatOpensNewWindow(),
                        Node()
                            .WithTitle("Administration").WithDescriptionSameAsTitle()
                            .ForController("Admin").ForAction("Index").WithUrlFromMvc()
                            .IfInRole("Admin"));
        }
    }
}
```

Following are the extension methods provided by FluentSiteMap that you can use to build your site map.  Just about every extension method has an override that takes a lambda expression instead of a direct value, which allows data loading to be deferred to when the site map is actually built instead of when the site map class is first created (see [Under The Hood](#under-the-hood) for more information on the design and life cycle of FluentSiteMap).
* `WithTitle` - Set the node title
* `WithDescription` - Set the node description
* `WithDescriptionSameAsTitle` - Simply use the title as the description
* `WithUrl` - Provide a specific URL for the node (alternative is to use the MVC controller/action methods described below)
* `WithTarget` - Set the URL target
* `WithTargetThatOpensNewWindow` - An easy way to do `WithTarget` that opens a new window
* `ForController` - Specify that the node will get its URL from a named MVC controller
* `ForAction` - Specify that the node will gets its URL from a named MVC action (used with `ForController`).  **TIP:** Notice that you can specify `WithController` once with a parent node and `WithAction` multiple times with each child node.
* `WithUrlFromMvc` - Used in conjunction with the `ForController` and `ForAction` methods to generate the final URL for the node; can also include additional route values for dynamically generated nodes
* `WithChildren` - Provide child nodes for the current node
* `WithMetadata` - Add custom metadata to the current node; useful when developing your own filters or custom partial views
* `HiddenInMenu` - Specify that the node is hidden when rendered in a menu (using the `FluentSiteMapMenu` partial views installed in `Views/Shared`)
* `HiddenInBreadCrumbs` - Specify that the node is hidden when rendered in bread crumbs (using the `FluentSiteMapBreadCrumbs` partial view installed in `Views/Shared`)
* `IfInRole` - The node will only be active if the current user is in one or more specified roles
* `IfNotInRole` - Opposite of `IfInRole`
* `IfAuthenticated` - The node will only be active if the user is authenticated
* `IfNotAuthenticated` - Opposite of `IfAuthenticated`

## Testing Reference

### Testing Extension Methods

Testing a site map involves asserting that each node in your site map configuration behaves as you expect it under various HTTP context scenarios.  Like most of MVC, FluentSiteMap uses a [RequestContext](http://msdn.microsoft.com/en-us/library/system.web.routing.requestcontext.aspx) instance to determine everything it needs to know about the current HTTP request, the user's identity, and relevant routing information.  Therefore, properly building a `RequestContext` object is the key to easily testing your site map.

The [FluentSiteMap Testing](http://nuget.org/packages/TS.FluentSiteMap.Testing) package contains a set of extension methods that help you do just that.  The pattern is to create a new `RequestContext` instance and chain together one or more of the following extension methods to arrange it appropriately for your test:

* `WithHttpContext` - Configures a `RequestContext` to simulate a specific request URL
* `WithRouting` - Configures a `RequestContext` so that it can be used to generate URL's from a routing table; must be used in conjunction with actually building a routing table
* `WithAuthenticatedUser` - Configures a `RequestContext` so that the current user is authenticated
* `WithUnauthenticatedUser` - Opposite of `WithAuthenticatedUser`
* `WithUserInRole` - Configures a `RequestContext` so that the current user is in a specific security role
* `WithUserNotInRole` - Opposite of `WithUserInRole`

Finally, you can generate an output node (which is actually a [FilteredNode](/src/FluentSiteMap.Core/FilteredNode.cs) object) that your test can assert against.  This can be done with one the following extension methods, which all take the `RequestContext` you've been building up so far and the site map instance that's under test:

* `GetCurrentNode`
* `GetRootNode`

Here's a snippet from a test method that uses many of these extension methods together to generate a root node that can then be asserted on:

```cs
// create an instance of the site map under test
var siteMap = new MySiteMap();

// register the routes (similar to your website) necessary for the test
RouteTable.Routes.Clear();
RouteTable.Routes.MapRoute(
    "Default", 
    "{controller}/{action}/{id}", 
    new { controller = "Home", action = "Index", id = UrlParameter.Optional } 

// get root node
var result = new RequestContext()
    .WithHttpContext("http://foo.com/")
    //NOTE: WithRouting 
    .WithRouting()
    .WithAuthenticatedUser()
    .WithUserInRole("Admin")
    .GetRootNode(siteMap); 

// perform asserts
...
```

To see a more extensive example of using these test helpers, take a look at the [FluentSiteMap.Sample.UnitTest](/src/FluentSiteMap.Sample.UnitTest) project in the repo.  To make state-based testing easier, those test methods make heavy use of the `ContainsState.With` NUnit syntax, which is made available via the [NUnitExtensions](https://github.com/twistedstream/NUnitExtensions) library.

### DecoratingNodeBuilderTestHelper Class

There's one other class contained within the Testing package that is specifically useful for testing custom Node Builder classes, which usually always inherit from [DecoratingNodeBuilder](/src/FluentSiteMap.Core/DecoratingNodeBuilder.cs).  The [DecoratingNodeBuilderTestHelper](/src/FluentSiteMap.Testing/DecoratingNodeBuilderTestHelper.cs) class provides a stubbed `BuilderContext`, inner `Node`, and inner `INodeBuilder` for your tests.  For examples on how to use this test class, see the [TS.FluentSiteMap.UnitTest.Builders](/tree/master/src/FluentSiteMap.UnitTest/Builders) namespace in the repo.

## Questions?
[@twistedstream](http://twitter.com/twistedstream)
