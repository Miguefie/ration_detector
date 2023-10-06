using Redis.OM.Modeling;

namespace RationDetectorAPI.Models;

[Document(StorageType = StorageType.Hash,Prefixes = new [] {"Measure"})]
public class Measure
{
    [Indexed]
    public Guid Identifier { get; set; }
    [Indexed]
    public Guid SiloIdentifier { get; set; }
    [Indexed]
    public decimal Distance { get; set; }
    [Indexed]
    public DateTime CreationDate { get; set; }
}