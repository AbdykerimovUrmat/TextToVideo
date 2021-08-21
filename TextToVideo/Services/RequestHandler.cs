using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Models;
using System.Collections.Generic;
using Uploader.Helpers;
using System.Linq;
using Microsoft.Extensions.Options;
using Models.Options;

namespace Uploader.Services
{
    public class RequestHandler : IHostedService
    {
        private Timer Timer { get; set; }

        private RequestService RequestService { get; set; }

        private VideoService VideoService { get; set; }

        private ImageService ImageService { get; set; }

        private AudioService AudioService { get; set; }

        private MediaOptions MediaOptions { get; set; }

        public RequestHandler(IServiceProvider serviceProvider, IOptions<MediaOptions> options)
        {
            var scope = serviceProvider.CreateScope();

            RequestService = scope.ServiceProvider.GetService<RequestService>();
            VideoService = scope.ServiceProvider.GetService<VideoService>();
            ImageService = scope.ServiceProvider.GetService<ImageService>();
            AudioService = scope.ServiceProvider.GetService<AudioService>();
            MediaOptions = options.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var publishTimeSpan = new TimeSpan(14, 0, 0); // 20:00:00 UTC+6, or 14:00:00 UTC
            var dueTime = publishTimeSpan - DateTime.UtcNow.TimeOfDay;
            
            if(dueTime <= new TimeSpan(0, 0, 0))
            {
                dueTime += new TimeSpan(24, 0, 0);
            }

            Timer = new Timer(DoWork, null, dueTime, TimeSpan.FromMinutes(MediaOptions.UpdatePerMinutes));
            return Task.CompletedTask;
        }

        public async void DoWork(object state)
        {
            var requests = await RequestService.Get<RequestModel.Get>();
            var font = new Font("Arial", 10, FontStyle.Italic);
            var utcNowString = DateTime.UtcNow.ToString("dd-MM-yyyy_HH-mm-ss");
            var basePath = $"C:\\images\\{utcNowString}\\";
            Directory.CreateDirectory(basePath + "images");
            Directory.CreateDirectory(basePath + "voices");
            Directory.CreateDirectory(basePath + "videos");
            Directory.CreateDirectory(basePath + "result");
            List<(Image, float)> images = new();
            for (int i = 0; i < requests.Count; i++)
            {
                var img = ImageService.TextToImage(requests[i].Text);
                img.Save(basePath + $"images\\{i}.jpg");

                AudioService.TextToAudio(basePath + $"voices\\{i}.wav", requests[i].Text);
                images.Add((img, AudioHelper.GetSoundLength(basePath + $"voices\\{i}.wav") / 1000f));
            }

            VideoService.CreateVideoFromList(images, 1440, 720, basePath + "videos\\video.avi", 24);
            AudioService.ListToAudio(basePath + "voices\\result.wav", requests.Select(x => x.Text));
            VideoService.MergeVideoAudio(basePath + "voices\\result.wav", basePath + "videos\\video.avi", basePath + "result\\result.avi");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}
