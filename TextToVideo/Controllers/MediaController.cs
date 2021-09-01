using Microsoft.AspNetCore.Mvc;
using Uploader.Helpers;
using Uploader.Infrastructure;
using Uploader.Services;

namespace Uploader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthorizeRoles("Admin")]
    public class MediaController : ControllerBase
    {
        private ImageService ImageService { get; }

        private AudioService AudioService { get; }

        private VideoService VideoService { get; }

        public MediaController(ImageService imageService, AudioService audioService, VideoService videoService)
        {
            ImageService = imageService;
            VideoService = videoService;
            AudioService = audioService;
        }

        [HttpGet]
        [Route("Audio")]
        public FileContentResult TextToAudio(string text)
        {
            AudioService.TextToAudio(@"C:\images\voice.wav", text);
            var audio = System.IO.File.ReadAllBytes(@"C:\images\voice.wav");
            return File(audio, "audio/wav");
        }

        [HttpGet]
        [Route("Image")]
        public FileContentResult TextToImage(string text)
        {
            var image = ImageService.TextToImage(text);

            return File(image.ToBytes(), "image/png");
        }

        [HttpGet]
        [Route("Video")]
        public void TextToVideo(string text)
        {
            AudioService.TextToAudio(@"C:\images\voice.wav", text);
            var videoDuration = AudioHelper.GetSoundLength(@"C:\images\voice.wav") / 1000.0f;
            var image = ImageService.TextToImage(text);
            image.Save(@"C:\images\0.jpg");
            VideoService.CreateVideo(image, @"C:\images\", "video.avi", 24, (int)videoDuration + 1);
            VideoService.MergeVideoAudio(@"C:\images\video.avi", @"C:\images\voice.wav", @"C:\images\result.avi");
            //var video = await System.IO.File.ReadAllBytesAsync(@"C:\images\result.avi");
            /*var uploader = new YouTubeUploader();
            return "http://youtube.com/watch?v=" + uploader.Upload();*/
        }
    }
}
