namespace RationDetectorAPI.Models;

public class Measure
{
    public Guid Identifier { get; set; }
    public Guid SiloIdentifier { get; set; }
    public decimal Distance { get; set; }
    public DateTime CreationDate { get; set; }
}