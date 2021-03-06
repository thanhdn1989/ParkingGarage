using Microsoft.Extensions.DependencyInjection;
using ParkingGarage.Core;
using ParkingGarage.Core.ParkingRecords;
using ParkingGarage.Infrastructure.Persistence;
using ParkingGarage.Infrastructure.Providers;
using ParkingGarage.Infrastructure.Repositories;
using ParkingGarage.Infrastructure.Validators;

namespace ParkingGarage.Infrastructure.Extensions;

public static class InfrastructureBootstrapper
{
    public static void RegisterInfrastructure(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IParkingRecordRepository, ParkingRecordRepository>();
        serviceCollection.AddScoped<IParkingValidator, ParkingRecordValidator>();
        serviceCollection.AddSingleton<IParkingDesignProvider, ParkingDesignProvider>();
        serviceCollection.AddSingleton<MainContext>();
    }
}