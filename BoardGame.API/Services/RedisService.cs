using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGame.API.Services
{
    public class RedisService : IRedisService
    {
        private readonly string _redisconnectionString;
        private readonly ILogger<RedisService> _logger;
        private ConnectionMultiplexer _redis;

        public RedisService(IConfiguration config, ILogger<RedisService> logger)
        {
            _redisconnectionString = config["Redis:ConnectionString"];
            _logger = logger;
        }

        public void Connect()
        {
            try
            {
                var configString = $"{_redisconnectionString},connectRetry=5";
                _redis = ConnectionMultiplexer.Connect(configString);
                _logger.LogInformation("Connected  to Redis");
            }
            catch (RedisConnectionException ex)
            {
                _logger.LogError("Error connecting to Redis" + ex.Message);
                throw;
            }
           
        }

        public async Task<bool> Set<T>(string key, T value)
        {
            var db = _redis.GetDatabase();
            return await db.StringSetAsync(key, JsonConvert.SerializeObject(value));
        }

        public async Task<T> Get<T>(string key)
        {
            var db = _redis.GetDatabase();
            var result = await db.StringGetAsync(key);
            if (result.IsNullOrEmpty)
                return default(T);
            else
                return JsonConvert.DeserializeObject<T>(result);
        }

    }
}
