using RationDetectorAPI.Models;
using Redis.OM;
using StackExchange.Redis;

namespace RationDetectorAPI;

public static class ServicesConfiguration
{
    //Representa o Proprio Service como classe
    public class Self { }
    
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = configuration["ConnectionStrings:Redis"]
                              ?? throw new ArgumentNullException("Redis Connection String was not provided");
        
        var multiplexer = ConnectionMultiplexer.Connect(redisConnection);
        
        services.AddSingleton(new RedisConnectionProvider(multiplexer));
     
        
        //Creation Of Silo (Init of Application)
        Silo silo = new Silo();
        silo.Identifier = Guid.Parse(configuration["Silo:Identifier"]
                                     ?? throw new ArgumentNullException("Silo Identifier was not provided!"));
        
        silo.Name = configuration["Silo:Name"]
                                     ?? throw new ArgumentNullException("Silo Name was not provided!");

        services.AddSingleton<Silo>(silo);


        services.AddHostedService<IndexCreationService>();
        

        return services;
    }
}