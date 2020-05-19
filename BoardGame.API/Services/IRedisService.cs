using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGame.API.Services
{
    public interface IRedisService
    {
        public void Connect();
        public  Task<bool> Set<T>(string key, T value);

        public Task<T> Get<T>(string key);
    }
}
