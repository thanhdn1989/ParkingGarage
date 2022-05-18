using ParkingGarage.Core.ParkingRecords;

namespace ParkingGarage.Infrastructure.Persistence;

/// <summary>
/// Simulate a persistence store, in real application: we will use DB instead
/// In this case i will create in-memory collections and use them for CRUD operation
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class MainContext 
{
    public List<ParkingRecord> ParkingRecords = new();

    /// <summary>
    /// I think we can use SQL with these 2 tables partitioned by EntryTime
    /// and use partition switching (if we have money for Enterprise edition :D )
    /// or have a worker to transfer data daily from ParkingRecords table to this history table
    /// </summary>
    public List<ParkingRecord> ParkingRecordsHist = new();
}