using ParkingGarage.Core.Options;

namespace ParkingGarage.Core;

/// <summary>
/// Support build garage structure from different sources
/// </summary>
public interface IParkingDesignProvider
{
    Task<ParkingGarageOptions> GetInitialDesignAsync();
}