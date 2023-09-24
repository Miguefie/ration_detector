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
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        

        return services;
    }
}