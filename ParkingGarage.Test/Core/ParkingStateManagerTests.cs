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
    private readonly Mock<IParkingDesignProvider> _parkingDesignProvider;

    public ParkingStateManagerTests()
    {
        _parkingRecordRepository = new Mock<IParkingRecordRepository>();
        _parkingDesignProvider = new Mock<IParkingDesignProvider>();
    }

    private async Task<ParkingGarageStateManager> CreateSut()
    {
        var sut = new ParkingGarageStateManager(_parkingRecordRepository.Object, _parkingDesignProvider.Object);
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
        _parkingDesignProvider.Setup(p => p.GetInitialDesignAsync())
            .ReturnsAsync(config);

        // Act
        var sut = await CreateSut();

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

        _parkingDesignProvider.Setup(p => p.GetInitialDesignAsync())
            .ReturnsAsync(config);

        // Act
        var sut = await CreateSut();

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
        _parkingDesignProvider.Setup(p => p.GetInitialDesignAsync())
            .ReturnsAsync(config);
        
        var sut = await CreateSut();

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
        _parkingDesignProvider.Setup(p => p.GetInitialDesignAsync())
            .ReturnsAsync(config);
        var sut = await CreateSut();

        // Verify
        sut.ReleaseParkingLot(VehicleType.Car);
        sut.ReleaseParkingLot(VehicleType.Motorbike);
    }
}