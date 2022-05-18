namespace ParkingGarage.Core.ParkingRecords;

public class ParkingRecord : Entity
{
    public Vehicles.Vehicle Vehicle { get; private set; }

    /// <summary>
    /// Indicate the time when vehicle comes in garage
    /// </summary>
    public DateTimeOffset EntryTime { get; private set; }
    
    /// <summary>
    /// Indicate the time when vehicle comes out garage
    /// </summary>
    public DateTimeOffset LeaveTime { get; private set; }

    public ParkingRecordStatus Status { get; private set; }

    public ParkingRecord(Vehicles.Vehicle vehicle) : base(Guid.NewGuid().ToString("N"))
    {
        Vehicle = vehicle;
        EntryTime = DateTimeOffset.UtcNow;
        Status = ParkingRecordStatus.Active;
    }

    public void UpdateLeaveTime(DateTimeOffset value)
    {
        LeaveTime = value;
        Status = ParkingRecordStatus.Inactive;
    }
}