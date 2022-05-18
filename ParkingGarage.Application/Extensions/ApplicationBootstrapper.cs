using Microsoft.Extensions.DependencyInjection;
using ParkingGarage.Application.UseCases;

namespace ParkingGarage.Application.Extensions;

public static class ApplicationBootstrapper
{
    public static void RegisterApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<VehicleEnterGarage>();
        serviceCollection.AddTransient<VehicleLeaveGarage>();
    }
}