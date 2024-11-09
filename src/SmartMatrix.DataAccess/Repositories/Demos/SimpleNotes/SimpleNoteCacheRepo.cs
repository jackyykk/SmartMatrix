using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNotes;
using SmartMatrix.Core.Validations;
using SmartMatrix.DataAccess.Constants;
using SmartMatrix.Domain.Demos.SimpleNotes.Entities;

namespace SmartMatrix.DataAccess.Repositories.Demos.SimpleNotes
{
    public class SimpleNoteCacheRepo : ISimpleNoteCacheRepo
    {
        private readonly IDistributedCache _cache;
        private readonly ISimpleNoteRepo _repo;

        public SimpleNoteCacheRepo(IDistributedCache cache, ISimpleNoteRepo repo)
        {
            _cache = cache;
            _repo = repo;
        }

        public async Task<SimpleNote?> GetByIdAsync(int id)
        {            
            SimpleNote? entity = null;
            var cacheKey = $"SimpleNote_Id_{id}";

            var bytes = await _cache.GetAsync(cacheKey);
            if (bytes != null)
            {
                var serializedEntity = Encoding.UTF8.GetString(bytes);
                entity = JsonSerializer.Deserialize<SimpleNote>(serializedEntity);
            }
            else
            {
                entity = await _repo.GetByIdAsync(id);
                // Save To Cache
                var serializedEntity = JsonSerializer.Serialize(entity);
                bytes = Encoding.UTF8.GetBytes(serializedEntity);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.UtcNow.AddSeconds(CacheConstants.DistributedCacheOptions.AbsoluteExpirationInSeconds))
                    .SetSlidingExpiration(TimeSpan.FromSeconds(CacheConstants.DistributedCacheOptions.SlidingExpirationInSeconds));

                await _cache.SetAsync(cacheKey, bytes, options);
            }

            return entity;
        }

        public async Task<List<SimpleNote>> GetListAsync(string owner)
        {
            var cacheKey = $"SimpleNotes_Owner_{owner}";
            List<SimpleNote> list = new List<SimpleNote>();

            var bytes = await _cache.GetAsync(cacheKey);
            if (bytes != null)
            {
                var serializedList = Encoding.UTF8.GetString(bytes);
                list = JsonSerializer.Deserialize<List<SimpleNote>>(serializedList) ?? new List<SimpleNote>();                
            }
            else
            {
                list = await _repo.GetListAsync(owner);
                // Save To Cache
                var serializedList = JsonSerializer.Serialize(list);
                bytes = Encoding.UTF8.GetBytes(serializedList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.UtcNow.AddSeconds(CacheConstants.DistributedCacheOptions.AbsoluteExpirationInSeconds))
                    .SetSlidingExpiration(TimeSpan.FromSeconds(CacheConstants.DistributedCacheOptions.SlidingExpirationInSeconds));

                await _cache.SetAsync(cacheKey, bytes, options);
            }

            return list;
        }
    }
}