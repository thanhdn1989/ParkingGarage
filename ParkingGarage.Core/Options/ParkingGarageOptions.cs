using System.ComponentModel.DataAnnotations;
using ParkingGarage.Core.Vehicles;

namespace ParkingGarage.Core.Options;

public class Garage
{
    [MaxLength(255)]
    [Required]
    public string Name { get; set; } = default!;

    public string? Location { get; set; } = default;
    
    [Required] public Level[] Levels { get; set; } = Array.Empty<Level>();
}

public class Specification
{
    public VehicleType Type { get; set; }
    public uint Capacity { get; set; }
}

public class Level
{
    [Required] public uint Index { get; set; } = 1;
    
    [MaxLength(255)] public string? Name { get; set; } = default;

    [Required] public Specification[] ParkingSpecification { get; set; } = Array.Empty<Specification>();

}

public class ParkingGarageOptions
{
    public Garage Garage { get; set; } = default!;
}
