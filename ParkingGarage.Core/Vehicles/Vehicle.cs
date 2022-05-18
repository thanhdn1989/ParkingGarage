namespace ParkingGarage.Core.Vehicles;

/// <summary>
/// Represent a vehicle instance
/// As current assignment i will keep this class as minimal as possible
/// </summary>
public abstract class Vehicle : Entity
{
    protected Vehicle(string licencePlateNumber) : base(licencePlateNumber)
    {
        LicencePlateNumber = licencePlateNumber;
    }

    public abstract VehicleType Type { get; }
    
    public string LicencePlateNumber { get; private set; }

}