using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

public class RedisConnectionService
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public RedisConnectionService(string redisConnectionString)
    {
        _redis = ConnectionMultiplexer.Connect(redisConnectionString);
        _db = _redis.GetDatabase();
    }

    public void SetString(string key, string value)
    {
        _db.StringSet(key, value);
    }

    public string? GetString(string key)
    {
        return _db.StringGet(key);
    }
}
