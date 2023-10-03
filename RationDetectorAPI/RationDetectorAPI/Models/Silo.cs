namespace RationDetectorAPI.Models;

public class Silo
{
    /// <summary>
    /// GUID Generated in Main Server
    /// </summary>
    public Guid Identifier { get; set; }
    /// <summary>
    /// Name of Silo
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// List of Measures Associated with the Silo
    /// </summary>
    public List<Measure> Measures { get; set; }
}