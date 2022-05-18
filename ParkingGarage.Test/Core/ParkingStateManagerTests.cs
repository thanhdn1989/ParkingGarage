using Microsoft.Extensions.Options;
using Moq;
using ParkingGarage.Core;
using ParkingGarage.Core.Options;
using ParkingGarage.Core.ParkingRecords;
using ParkingGarage.Core.Vehicles;
using ParkingGarage.Test.Data;
using Xunit;

namespace ParkingGarage.Test.Core;

public class ParkingStateManagerTests
{
    private readonly Mock<IParkingRecordRepository> _parkingRecordRepository;

    public ParkingStateManagerTests()
    {
        _parkingRecordRepository = new Mock<IParkingRecordRepository>();
    }

    private async Task<ParkingGarageStateManager> CreateSut(IOptions<ParkingGarageOptions> options)
    {
        var sut = new ParkingGarageStateManager(options, _parkingRecordRepository.Object);
        await sut.BootstrapAsync();
        return sut;
    }

    [Fact]
    public async Task AssignParkingLot_WhenThereIsNoFreeSpace_ShouldRaiseError()
    {
        // Arrange
        var config = ParkingGarageConfig.CreateSample(1, 2, 2);
        var vehicle = new Car(Guid.NewGuid().ToString());
        _parkingRecordRepository.Setup(p => p.CollectAllActiveParkingRecordsAsync())
            .ReturnsAsync(new Dictionary<VehicleType, uint>
            {
                { VehicleType.Car, 2 }
            });

        // Act
        var sut = await CreateSut(Options.Create(config));

        // Verify
        Assert.Throws<BusinessException>(() => sut.FindAndUpdateFreeParkingLot(vehicle.Type));
    }

    [Fact]
    public async Task AssignParkingLot_WhenThereIsFreeSpace_ShouldReturnTrue()
    {
        // Arrange
        var config = ParkingGarageConfig.CreateSample(1, 2, 2);
        var vehicle = new Car(Guid.NewGuid().ToString());
        _parkingRecordRepository.Setup(p => p.CollectAllActiveParkingRecordsAsync())
            .ReturnsAsync(new Dictionary<VehicleType, uint>
            {
                { VehicleType.Car, 1 }
            });

        // Act
        var sut = await CreateSut(Options.Create(config));

        // Verify
        sut.FindAndUpdateFreeParkingLot(vehicle.Type);
    }

    [Fact]
    public async Task ReleaseParkingLot_WhenThereIsNoActiveParkingRecord_ShouldRaiseError()
    {
        // Arrange
        var config = ParkingGarageConfig.CreateSample(1, 2, 2);
        _parkingRecordRepository.Setup(p => p.CollectAllActiveParkingRecordsAsync())
            .ReturnsAsync(new Dictionary<VehicleType, uint>());
        var sut = await CreateSut(Options.Create(config));

        // Verify
        Assert.Throws<BusinessException>(() => sut.ReleaseParkingLot(VehicleType.Car));
        Assert.Throws<BusinessException>(() => sut.ReleaseParkingLot(VehicleType.Motorbike));
    }

    [Fact]
    public async Task ReleaseParkingLot_WhenThereIsActiveParkingRecord_ShouldNotRaiseError()
    {
        // Arrange
        var config = ParkingGarageConfig.CreateSample(1, 2, 2);
        _parkingRecordRepository.Setup(p => p.CollectAllActiveParkingRecordsAsync())
            .ReturnsAsync(new Dictionary<VehicleType, uint>
            {
                { VehicleType.Car, 2 },
                { VehicleType.Motorbike, 2 }
            });
        var sut = await CreateSut(Options.Create(config));

        // Verify
        sut.ReleaseParkingLot(VehicleType.Car);
        sut.ReleaseParkingLot(VehicleType.Motorbike);
    }
}