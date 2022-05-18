using ParkingGarage.Core.Vehicles;

namespace ParkingGarage.Core.ParkingRecords;

public class ParkingService
{
    private readonly IParkingRecordRepository _parkingRecordRepository;
    private readonly IParkingValidator _parkingRecordValidator;
    private readonly ParkingGarageStateManager _parkingGarageStateManager;

    public ParkingService(IParkingRecordRepository parkingRecordRepository, IParkingValidator parkingRecordValidator, 
        ParkingGarageStateManager parkingGarageStateManager)
    {
        _parkingRecordRepository = parkingRecordRepository;
        _parkingRecordValidator = parkingRecordValidator;
        _parkingGarageStateManager = parkingGarageStateManager;
    }

    /// <summary>
    /// Use to create the new parking record when it comes in garage
    /// </summary>
    /// <returns>Id (i assume we can use it as ticket number)</returns>
    /// <exception cref="BusinessException">
    /// Vehicle has active parking records
    /// There are no free parking lot
    /// </exception>
    public async Task<string> AssignParkingLotAsync(Vehicle vehicle)
    {
        await _parkingRecordValidator.EnsureVehicleCanEnterGarage(vehicle);
        _parkingGarageStateManager.FindAndUpdateFreeParkingLot(vehicle.Type);
        var record = new ParkingRecord(vehicle);
        await _parkingRecordRepository.InsertAsync(record);
        return record.Id;
    }

    /// <summary>
    /// Used to mark a vehicle leave garage
    /// </summary>
    /// <param name="vehicle"></param>
    public async Task MarkVehicleLeaveGarageAsync(Vehicle vehicle)
    {
        await _parkingRecordValidator.EnsureVehicleCanLeaveGarage(vehicle);
        var parkingRecord = await _parkingRecordRepository.FindParkingRecordAsync(vehicle);
        parkingRecord!.UpdateLeaveTime(DateTimeOffset.UtcNow);
        await _parkingRecordRepository.UpdateAsync(parkingRecord);
        _parkingGarageStateManager.ReleaseParkingLot(vehicle.Type);
    }
}