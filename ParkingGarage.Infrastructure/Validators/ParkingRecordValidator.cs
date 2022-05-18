using ParkingGarage.Core;
using ParkingGarage.Core.ParkingRecords;
using ParkingGarage.Core.Vehicles;

namespace ParkingGarage.Infrastructure.Validators;

public class ParkingRecordValidator : IParkingValidator
{
    private readonly IParkingRecordRepository _parkingRecordRepository;

    public ParkingRecordValidator(IParkingRecordRepository parkingRecordRepository)
    {
        _parkingRecordRepository = parkingRecordRepository;
    }

    public async Task EnsureVehicleCanEnterGarage(Vehicle vehicle)
    {
        // Check there is no active parking records of the vehicle
        var parkingRecord = await
            _parkingRecordRepository.FindParkingRecordAsync(vehicle);

        if (parkingRecord?.Status == ParkingRecordStatus.Active)
            throw new BusinessException(
                $"There is active parking record of this vehicle, license plate: {vehicle.LicencePlateNumber}");
    }
    
    public async Task EnsureVehicleCanLeaveGarage(Vehicle vehicle)
    {
        var parkingRecord = await
            _parkingRecordRepository.FindParkingRecordAsync(vehicle);

        if (parkingRecord == null)
            throw new BusinessException(
                $"There is no active parking record of this vehicle, license plate: {vehicle.LicencePlateNumber}");
        
        if (parkingRecord.Status == ParkingRecordStatus.Inactive)
            throw new BusinessException(
                $"There is inactive parking record of this vehicle, license plate: {vehicle.LicencePlateNumber}");

        if (parkingRecord.Vehicle.Type != vehicle.Type)
            throw new BusinessException($"There is another type of vehicle with same license plate {vehicle.LicencePlateNumber} in garage");
    }
}