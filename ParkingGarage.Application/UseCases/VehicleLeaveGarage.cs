using System.ComponentModel.DataAnnotations;
using ParkingGarage.Core.ParkingRecords;
using ParkingGarage.Core.Vehicles;

namespace ParkingGarage.Application.UseCases;

public class VehicleLeaveGarage : IUseCase<VehicleLeaveGarage.Request>
{
    private readonly ParkingService _parkingService;

    public VehicleLeaveGarage(ParkingService parkingService)
    {
        _parkingService = parkingService;
    }

    public async Task ExecuteAsync(Request request)
    {
        Vehicle vehicle = request.Type == VehicleType.Car
            ? new Car(request.LicensePlateNumber)
            : new Motorbike(request.LicensePlateNumber);
        await _parkingService.MarkVehicleLeaveGarageAsync(vehicle);
    }
    
    public class Request
    {
        public VehicleType Type { get; set; }

        [Required] public string LicensePlateNumber { get; set; } = default!;
    }
}