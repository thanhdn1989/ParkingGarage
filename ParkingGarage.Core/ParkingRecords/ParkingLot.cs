using System.ComponentModel.DataAnnotations;

namespace ParkingGarage.Core.ParkingRecords;

/// <summary>
/// For future use, if we need the ability for parking lot preservation
/// </summary>
public class ParkingLot
{
    /// <summary>
    /// The level index where vehicle is
    /// </summary>
    [Range(1, uint.MaxValue)]
    public uint Level { get; set; }
    
    /// <summary>
    /// Specific parking lot vehicle want to preserve
    /// </summary>
    [Range(1, uint.MaxValue)]
    public uint SpaceNo { get; set; }
}