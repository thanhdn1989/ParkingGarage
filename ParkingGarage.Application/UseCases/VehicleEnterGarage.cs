using System.ComponentModel.DataAnnotations;
using ParkingGarage.Core.ParkingRecords;
using ParkingGarage.Core.Vehicles;

namespace ParkingGarage.Application.UseCases;

public class VehicleEnterGarage : IUseCase<VehicleEnterGarage.Request, string>
{
    private readonly ParkingService _parkingService;
    

    public VehicleEnterGarage(ParkingService parkingService)
    {
        _parkingService = parkingService;
    }
    
    public async Task<string> ExecuteAsync(Request request)
    {
        Vehicle vehicle = request.Type == VehicleType.Car
            ? new Car(request.LicensePlateNumber)
            : new Motorbike(request.LicensePlateNumber);
        return await _parkingService.AssignParkingLotAsync(vehicle);
    }
    
    public class Request
    {
        public VehicleType Type { get; set; }
        [Required] public string LicensePlateNumber { get; set; } = default!;
    }

}