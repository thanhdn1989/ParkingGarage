namespace ParkingGarage.Core.Vehicles;

public class Car : Vehicle
{
    public override VehicleType Type => VehicleType.Car;

    public Car(string licencePlateNumber) : base(licencePlateNumber)
    {
        
    }
}