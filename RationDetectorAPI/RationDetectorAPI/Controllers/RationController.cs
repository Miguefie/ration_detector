using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NRedisStack;
using RationDetectorAPI.DTOs;
using RationDetectorAPI.Models;
using Redis.OM;
using Redis.OM.Searching;
using StackExchange.Redis;

namespace RationDetectorAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RationController : ControllerBase
{
    private readonly Silo _silo;
    private readonly RedisCollection<Measure> _measures;
    private readonly IDatabase _db;
    private readonly IServer _server;

    
    public RationController(Silo silo, RedisConnectionProvider provider, IDatabase db, IServer server)
    {
        _silo = silo;
        _measures = (RedisCollection<Measure>)provider.RedisCollection<Measure>();
        _db = db;
        _server = server;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("measures")]
    public async Task<ActionResult<List<Measure>>> GetSiloData()
    {
        var redisResult = _server.Keys(pattern:"*");

        var measures = new List<Measure>();   
        
        foreach (var key in redisResult)
        {
            var measure = new Measure();
            
            // Check if the key type is a hash
            if (_db.KeyType(key) == RedisType.Hash)
            {
                // Use HGETALL to get all fields and values for the hash key
                var hashEntries = await _db.HashGetAllAsync(key);
                
                foreach (var hash in hashEntries)
                {
                    switch (hash.Name)
                    {
                        case "Identifier":
                            measure.Identifier = Guid.Parse(hash.Value.ToString());
                            break;
                        case "SiloIdentifier":
                            measure.SiloIdentifier = Guid.Parse(hash.Value.ToString());
                            break;
                        case "Distance":
                            measure.Distance = Decimal.Parse(hash.Value.ToString(), CultureInfo.InvariantCulture);
                            break;
                        case "CreationDate":
                            measure.CreationDate = hash.Value.ToString();
                            break;
                    }
                }
            }
            
            measures.Add(measure);
        }
        
        return Ok(measures);
    }
    
    [AllowAnonymous]
    [HttpDelete]
    [Route("measures")]
    public ActionResult<Silo> DeleteSiloData()
    {
        try
        {
            _db.Execute("FLUSHDB");
            return Ok(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e);
        }
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("silo")]
    public async Task<ActionResult<Silo>> CreateMeasure([FromBody] MeasureDTO measureDto)
    {
        try
        {
            if (measureDto != null)
            {
                var measure = new Measure();
                measure.Identifier = Guid.NewGuid();
                measure.SiloIdentifier = _silo.Identifier;
                measure.Distance = measureDto.Distance;
                measure.CreationDate = measureDto.CreationDate;

                _measures.Insert(measure);

                return Ok(measure);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e);
        }
        

        return BadRequest("Not Inserted");
    }
}