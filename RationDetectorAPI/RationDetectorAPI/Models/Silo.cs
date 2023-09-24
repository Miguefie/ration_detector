namespace RationDetectorAPI.Models;

public class Silo
{
    public Guid Identifier { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; }
    public List<Measure> Measures { get; set; }
}