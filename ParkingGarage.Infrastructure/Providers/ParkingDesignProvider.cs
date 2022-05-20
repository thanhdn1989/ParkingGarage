using Microsoft.Extensions.Options;
using ParkingGarage.Core;
using ParkingGarage.Core.Options;

namespace ParkingGarage.Infrastructure.Providers;

public class ParkingDesignProvider : IParkingDesignProvider
{
    private readonly ParkingGarageOptions _options;
    public ParkingDesignProvider(IOptions<ParkingGarageOptions> options)
    {
        _options = options.Value;
    }
    public Task<ParkingGarageOptions> GetInitialDesignAsync()
    {
        return Task.FromResult(_options);
    }
}