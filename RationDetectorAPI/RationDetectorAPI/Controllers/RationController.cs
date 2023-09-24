using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RationDetectorAPI.Models;
using StackExchange.Redis;

namespace RationDetectorAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RationController : ControllerBase
{
    private readonly IDatabase _redisDB;
    
    public RationController(IConnectionMultiplexer redis)
    {
        _redisDB = redis.GetDatabase();
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
    public ActionResult<Silo> CreateSilo(Silo silo)
    {
        return Ok("TESTE");
    }
}