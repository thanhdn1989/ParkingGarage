using ParkingGarage.Core.Vehicles;

namespace ParkingGarage.Core.ParkingRecords;

public interface IParkingValidator
{
    /// <summary>
    /// Make sure that vehicle can enter the garage by checking if it has parking record before and
    /// that record is active
    /// </summary>
    /// <param name="vehicle"></param>
    /// <exception cref="BusinessException">
    ///     + When parking record is found
    ///     + There is already active parking record
    /// </exception>
    Task EnsureVehicleCanEnterGarage(Vehicle vehicle);

    /// <summary>
    /// Make sure that vehicle can enter the garage by checking if it has parking record before and
    /// that record is inactive
    /// </summary>
    /// <param name="vehicle"></param>
    /// <exception cref="BusinessException">
    ///     + When parking record is not found
    ///     + There is already inactive parking record
    /// </exception>
    Task EnsureVehicleCanLeaveGarage(Vehicle vehicle);
}