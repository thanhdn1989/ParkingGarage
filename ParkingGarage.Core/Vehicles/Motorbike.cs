namespace ParkingGarage.Core.Vehicles;

public class Motorbike : Vehicle
{
    public Motorbike(string licencePlateNumber) : base(licencePlateNumber)
    {
    }

    public override VehicleType Type => VehicleType.Motorbike;
}