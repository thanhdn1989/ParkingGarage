using Microsoft.Extensions.Options;
using ParkingGarage.Core.Options;
using ParkingGarage.Core.ParkingRecords;
using ParkingGarage.Core.Vehicles;

namespace ParkingGarage.Core;

/// <summary>
/// Manage the current state of the parking garage
/// Will be the singleton class
/// No public interface, since this is pure business logic
/// </summary>
public class ParkingGarageStateManager
{
    private readonly Dictionary<VehicleType, uint> _freeSpaceState = new();
    
    /// <summary>
    /// Keep the initial design of garage for validation
    /// such as: invalid operation when try releasing parking lot but there is not any active parking record
    /// </summary>
    private readonly Dictionary<VehicleType, uint> _garageParkingLotDesign = new();
    private readonly ParkingGarageOptions _options;
    private readonly IParkingRecordRepository _parkingRecordRepository;

    // Used for preventing race condition
    private readonly Mutex _mutex = new();

    public ParkingGarageStateManager(IOptions<ParkingGarageOptions> options, IParkingRecordRepository parkingRecordRepository)
    {
        _options = options.Value;
        _parkingRecordRepository = parkingRecordRepository;
    }

    /// <summary>
    /// Populate the current state of parking garage (current free parking lot of the garage)
    /// </summary>
    public async Task BootstrapAsync()
    {
        InitializeGarageState();
        var recentActiveParkingRecords = await _parkingRecordRepository.CollectAllActiveParkingRecordsAsync();
        foreach (var specification in _garageParkingLotDesign)
        {
            var spaceInUse = recentActiveParkingRecords.ContainsKey(specification.Key)
                ? recentActiveParkingRecords[specification.Key]
                : 0;
            _freeSpaceState.Add(specification.Key, Math.Max(specification.Value - spaceInUse, 0));
        }
    }

    /// <summary>
    /// Initialize the garage's capacity
    /// </summary>
    private void InitializeGarageState()
    {
        var specifications = _options.Garage.Levels.SelectMany(l => l.ParkingSpecification)
            .ToList();
        foreach (var specification in specifications)
        {
            if (!_garageParkingLotDesign.ContainsKey(specification.Type))
                _garageParkingLotDesign.Add(specification.Type, specification.Capacity);
            else
                _garageParkingLotDesign[specification.Type] += specification.Capacity;
        }
    }

    /// <summary>
    /// Try to find an available parking lot
    /// </summary>
    /// <param name="vehicleType"></param>
    /// <exception cref="BusinessException">
    /// There is no free space left
    /// </exception>
    public void FindAndUpdateFreeParkingLot(VehicleType vehicleType)
    {
        try
        {
            _mutex.WaitOne();
            if (_freeSpaceState[vehicleType] <= 0)
                throw new BusinessException("There is no free space");

            _freeSpaceState[vehicleType]--;
        }
        finally
        {
            _mutex.ReleaseMutex();
        }
    }

    /// <summary>
    /// When ever a vehicle come out garage, we will increase the number of free parking lot
    /// </summary>
    /// <param name="vehicleType"></param>
    /// <exception cref="BusinessException">
    /// Free an invalid vehicle can cause the incorrect parking lot state
    /// </exception>
    public void ReleaseParkingLot(VehicleType vehicleType)
    {
        try
        {
            _mutex.WaitOne();
            if (_freeSpaceState[vehicleType] + 1 > _garageParkingLotDesign[vehicleType])
                throw new BusinessException("Invalid operation, can free more space");
            _freeSpaceState[vehicleType]++;
        }
        finally
        {
            _mutex.ReleaseMutex();
        }
    }
}