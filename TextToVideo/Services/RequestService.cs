using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.EF;
using DAL.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models.Options;

namespace Uploader.Services
{
    public class RequestService
    {
        private AppDbContext Context { get; set; }

        private MediaOptions MediaOptions { get; set; }

        public RequestService(AppDbContext context, IOptions<MediaOptions> mediaOptions)
        {
            Context = context;
            MediaOptions = mediaOptions.Value;
        }

        public async Task<int> Add<T>(T model)
        {
            var entity = model.Adapt<Request>();
            Context.Requests.Add(entity);
            await Context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<List<T>> Get<T>()
        {
            return await Context.Requests
                .AsNoTracking()
                .ProjectToType<T>()
                .ToListAsync();
        }

        public async Task<List<T>> GetUnused<T>(int topn)
        {
            return await Context.Requests
                .AsNoTracking()
                .Where(x => !x.IsUsed)
                .OrderBy(x => x.CreatedUtc)
                .Take(topn)
                .ProjectToType<T>()
                .ToListAsync();
        }

        public async Task SetUnused(int topn)
        {
            await Context.Requests
                .Where(x => !x.IsUsed)
                .OrderBy(x => x.CreatedUtc)
                .Take(topn)
                .ForEachAsync(x => x.IsUsed = true);
            Context.SaveChanges();
        }
    }
}
