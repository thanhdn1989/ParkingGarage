using ParkingGarage.Core.Options;
using ParkingGarage.Core.Vehicles;

namespace ParkingGarage.Test.Data;

public static class ParkingGarageConfig
{
    public static ParkingGarageOptions CreateSample(uint levels = 1, uint numberOfCar = 10, uint numberOfMotorbike = 10)
    {
        var levelList = new List<Level>();
        for (uint index = 1; index <= levels; index++)
        {
            levelList.Add(new Level
            {
                Index = index,
                ParkingSpecification = new []
                {
                    new Specification
                    {
                        Type = VehicleType.Car,
                        Capacity = numberOfCar
                    },
                    new Specification
                    {
                        Type = VehicleType.Motorbike,
                        Capacity = numberOfMotorbike
                    }
                }
            });
        }
        return new ParkingGarageOptions
        {
            Garage = new Garage
            {
                Levels = levelList.ToArray()
            }
        };
    } 
}