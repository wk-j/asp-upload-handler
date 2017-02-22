// Learn more about F# at http://fsharp.org

open System

open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open System.IO;

type HelloApiController() =
    inherit Controller()

    // Error: This localhost page can’t be found
    [<HttpGet>]
    [<Route("api/helloApi/hi")>]
    member this.Hi() = "Hello, World!"


type Startup(env: IHostingEnvironment) = 

    member val Configuration: IConfigurationRoot = null with set,get

    member this.ConfigureServices(services: IServiceCollection) =
        services.AddMvc()
        ()

    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment, logger: ILoggerFactory) =
        app.UseMvc()
        ()


[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    let host = 
        WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build();

    host.Run();
    0
