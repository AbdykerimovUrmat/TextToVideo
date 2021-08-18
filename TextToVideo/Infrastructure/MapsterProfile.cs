using System;
using DAL.Entities;
using Mapster;
using Models.Models;

namespace Uploader.Infrastructure
{
    public static class MapsterProfile
    {
        public static void Register()
        {
            TypeAdapterConfig<RequestModel.AddIn, Request>
                .NewConfig()
                .Map(dest => dest.CreatedUtc, src => DateTime.UtcNow);
        }
    }
}
