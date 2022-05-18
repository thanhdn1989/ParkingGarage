using ParkingGarage.Core.Vehicles;

namespace ParkingGarage.Core.ParkingRecords;

/// <summary>
/// 
/// </summary>
public interface IParkingRecordRepository
{
    Task UpdateAsync(ParkingRecord parkingRecord);
    
    Task InsertAsync(ParkingRecord parkingRecord);
    
    /// <summary>
    /// Use for checking if a car is already come in by its license plate number
    /// 
    /// In VietNam, license plate is unique for all kind of vehicle, but i assume we can have same license plate
    /// for each kind of vehicle
    /// </summary>
    /// <param name="vehicle"></param>
    /// <returns></returns>
    Task<ParkingRecord?> FindParkingRecordAsync(Vehicle vehicle);

    /// <summary>
    /// I have no idea of Vence parking garage working, but it seem in real life
    /// we will have a ticket (Parking Record id) whenever we come in the garage
    /// 
    /// In VietNam, license plate is unique for all kind of vehicle, but i assume we can have same license plate
    /// for each kind of vehicle
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ParkingRecord?> FindByIdAsync(string id);

    /// <summary>
    /// Get all current active parking records in persistence store
    /// </summary>
    /// <returns>Dictionary contains the active parking records of each level</returns>
    public Task<Dictionary<VehicleType, uint>> CollectAllActiveParkingRecordsAsync();
}