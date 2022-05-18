using ParkingGarage.Core.ParkingRecords;
using ParkingGarage.Core.Vehicles;
using ParkingGarage.Infrastructure.Persistence;

namespace ParkingGarage.Infrastructure.Repositories;

public class ParkingRecordRepository : IParkingRecordRepository
{
    private readonly MainContext _mainContext;

    public ParkingRecordRepository(MainContext mainContext)
    {
        _mainContext = mainContext;
    }

    public Task UpdateAsync(ParkingRecord parkingRecord)
    {
        var index = _mainContext.ParkingRecords.FindIndex(p => p.Id == parkingRecord.Id);
        _mainContext.ParkingRecords[index] = parkingRecord;
        return Task.CompletedTask;
    }

    public Task InsertAsync(ParkingRecord parkingRecord)
    {
        _mainContext.ParkingRecords.Add(parkingRecord);
        return Task.CompletedTask;
    }

    public Task<ParkingRecord?> FindParkingRecordAsync(Vehicle vehicle)
    {
        return Task.FromResult(_mainContext.ParkingRecords
            .OrderByDescending(p => p.EntryTime)
            .FirstOrDefault(p => p.Vehicle.LicencePlateNumber == vehicle.LicencePlateNumber));
    }

    public Task<Dictionary<VehicleType, uint>> CollectAllActiveParkingRecordsAsync()
    {
        return Task.FromResult(_mainContext.ParkingRecords
            .Where(p => p.Status == ParkingRecordStatus.Active)
            .GroupBy(p => p.Vehicle.Type)
            .ToDictionary(p => p.Key, p => (uint)p.Count()));
    }
    
    public Task<ParkingRecord?> FindByIdAsync(string id)
    {
        return Task.FromResult(_mainContext.ParkingRecords.FirstOrDefault(p => p.Id == id));
    }
}