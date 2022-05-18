using Microsoft.Extensions.DependencyInjection;
using ParkingGarage.Core.ParkingRecords;

namespace ParkingGarage.Core.Extensions;

public static class CoreBootstrapper
{
    public static void RegisterCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ParkingService>();
        serviceCollection.AddSingleton<ParkingGarageStateManager>();
    }
}