using Moq;
using ParkingGarage.Core;
using ParkingGarage.Core.ParkingRecords;
using ParkingGarage.Core.Vehicles;
using ParkingGarage.Infrastructure.Validators;
using Xunit;

namespace ParkingGarage.Test.Core;

public class ParkingValidatorTests
{
    private readonly Mock<IParkingRecordRepository> _parkingRecordRepository;

    public ParkingValidatorTests()
    {
        _parkingRecordRepository = new Mock<IParkingRecordRepository>();
    }

    public ParkingRecordValidator CreateSut()
    {
        return new ParkingRecordValidator(_parkingRecordRepository.Object);
    }

    [Fact]
    public async Task VehicleEnter_WithExistingParkingRecord_ShouldRaiseError()
    {
        // Arrange
        var sut = CreateSut();
        var vehicle = new Car(Guid.NewGuid().ToString());
        var parkingRecord = new ParkingRecord(vehicle);

        _parkingRecordRepository.Setup(p => p.FindParkingRecordAsync(vehicle))
            .ReturnsAsync(parkingRecord);
        
        // Verify
        await Assert.ThrowsAsync<BusinessException>(() => sut.EnsureVehicleCanEnterGarage(vehicle));

    }
    
    [Fact]
    public async Task VehicleEnter_WithExistingInActiveParkingRecord_ShouldNotRaiseError()
    {
        // Arrange
        var sut = CreateSut();
        var vehicle = new Car(Guid.NewGuid().ToString());
        var parkingRecord = new ParkingRecord(vehicle);
        parkingRecord.UpdateLeaveTime(DateTimeOffset.UtcNow);

        _parkingRecordRepository.Setup(p => p.FindParkingRecordAsync(vehicle))
            .ReturnsAsync(parkingRecord);
        
        // Verify
        await sut.EnsureVehicleCanEnterGarage(vehicle);
    }
    
    [Fact]
    public async Task VehicleLeave_WithNotExistingParkingGarage_ShouldRaiseError()
    {
        // Arrange
        var sut = CreateSut();
        var vehicle = new Car(Guid.NewGuid().ToString());

        _parkingRecordRepository.Setup(p => p.FindParkingRecordAsync(vehicle))
            .ReturnsAsync((ParkingRecord?)default);
        
        // Verify
        await Assert.ThrowsAsync<BusinessException>(() => sut.EnsureVehicleCanLeaveGarage(vehicle));
    }
    
    [Fact]
    public async Task VehicleLeave_WithExistingInActiveParkingRecord_ShouldRaiseError()
    {
        // Arrange
        var sut = CreateSut();
        var vehicle = new Car(Guid.NewGuid().ToString());

        _parkingRecordRepository.Setup(p => p.FindParkingRecordAsync(vehicle))
            .ReturnsAsync((ParkingRecord?)default);
        
        // Verify
        await Assert.ThrowsAsync<BusinessException>(() => sut.EnsureVehicleCanLeaveGarage(vehicle));
    }
    
    [Fact]
    public async Task VehicleLeave_WithExistingActiveParkingRecord_ShouldNotRaiseError()
    {
        // Arrange
        var sut = CreateSut();
        var vehicle = new Car(Guid.NewGuid().ToString());
        var parkingRecord = new ParkingRecord(vehicle);

        _parkingRecordRepository.Setup(p => p.FindParkingRecordAsync(vehicle))
            .ReturnsAsync(parkingRecord);
        
        // Verify
        await sut.EnsureVehicleCanLeaveGarage(vehicle);
    }
}