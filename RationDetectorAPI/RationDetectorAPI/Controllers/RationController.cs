using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    
    public RationController(Silo silo, RedisConnectionProvider provider)
    {
        _silo = silo;
        _measures = (RedisCollection<Measure>)provider.RedisCollection<Measure>();
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("silo")]
    public ActionResult<Silo> GetSiloData()
    {
        return Ok("TESTE");
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