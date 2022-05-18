using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParkingGarage.Application.Extensions;
using ParkingGarage.Application.UseCases;
using ParkingGarage.Core;
using ParkingGarage.Core.Extensions;
using ParkingGarage.Core.Options;
using ParkingGarage.Core.Vehicles;
using ParkingGarage.Infrastructure.Extensions;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, configuration) =>
    {
        configuration.Sources.Clear();

        var env = hostingContext.HostingEnvironment;

        configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("garagestructure.json", optional:false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
    })
    .ConfigureServices((builder, services) =>
    {
        services.Configure<ParkingGarageOptions>(options =>
        {
            options.Garage = builder.Configuration.GetSection("Garage").Get<Garage>();
        });
        services.RegisterCore();
        services.RegisterInfrastructure();
        services.RegisterApplication();
    })
    .Build();

await ExemplifyScoping(host.Services);
await host.RunAsync();


static async Task ExemplifyScoping(IServiceProvider services)
{
    using IServiceScope serviceScope = services.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;
    var stateManage = provider.GetService<ParkingGarageStateManager>();
    await stateManage!.BootstrapAsync();
    do {
        while (! Console.KeyAvailable) {
            Console.WriteLine($"Welcome. You are:{Environment.NewLine}1. Car {Environment.NewLine}2. Motorbike");
            var vehicleType = Console.ReadLine();
            
            Console.WriteLine($"You want to:{Environment.NewLine}1. Enter {Environment.NewLine}2. Leave");
            var option = Console.ReadLine();
            
            Console.WriteLine("Please enter the license plate number");
            var licensePlateNumber = Console.ReadLine();

            switch (option)
            {
                // Vehicle enter garage
                case "1":
                    try
                    {
                        var vehicleEnterGarageRequest = new VehicleEnterGarage.Request
                        {
                            Type = vehicleType == "1" ? VehicleType.Car : VehicleType.Motorbike,
                            LicensePlateNumber = licensePlateNumber!.Trim()
                        };
                        var vehicleEnterGarageUseCase = services.GetService<VehicleEnterGarage>();
                        var result = await vehicleEnterGarageUseCase!.ExecuteAsync(vehicleEnterGarageRequest);
                        Console.WriteLine($"Ticket number: {result}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Rejected. Reason: {e.Message}");
                    }
                    break;
                // Vehicle leave garage
                case "2":
                    try
                    {
                        var carLeaveRequest = new VehicleLeaveGarage.Request
                        {
                            Type = vehicleType == "1" ? VehicleType.Car : VehicleType.Motorbike,
                            LicensePlateNumber = licensePlateNumber!.Trim()
                        };
                        var vehicleLeaveGarageUseCase = services.GetService<VehicleLeaveGarage>();
                        await vehicleLeaveGarageUseCase!.ExecuteAsync(carLeaveRequest);
                        Console.WriteLine("Vehicle left");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Failed. Reason: {e.Message}");
                    }
                    break;
            }
        }       
    } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

}