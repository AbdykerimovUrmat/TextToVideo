using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Text;
using AForge.Video.FFMPEG;
using Microsoft.AspNetCore.Mvc;
using Uploader.Services;

namespace Uploader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        
        private ImageService ImageService { get; }
        public TestController(ImageService imageService) 
        {
            ImageService = imageService;
        }
        [HttpGet]
        public string Test(string text)
        {
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToWaveFile(@"C:\images\voice.wav");
            synthesizer.Speak(text);
            synthesizer.Dispose();
            var videoDuration = SoundInfo.GetSoundLength(@"C:\images\voice.wav") / 1000.0f;
            for (int i = 20; i < text.Length; i += 20)
            {
                text = text.Insert(i, "\n");
            }
            var image = ImageService.DrawText(text, new Font("Arial", 20), 1440, 720, Color.Black, Color.White);
            image.Save(@"C:\images\0.jpg");
            ImageService.CreateVideo(image, @"C:\images\", "video.avi", 24, (int) videoDuration + 1);

            string strCmdText;
            strCmdText = "ffmpeg -i \"C:\\images\\video.avi\" -i \"C:\\images\\voice.wav\" -shortest \"C:\\images\\result.avi\"";
            string args = "/c ffmpeg -i \"C:\\images\\video.avi\" -i \"C:\\images\\voice.wav\" -shortest \"C:\\images\\result.avi\"";
            ProcessStartInfo startInfo = new()
            {
                CreateNoWindow = false,
                FileName = @"C:\Users\User\Documents\ffmpeg.exe",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = string.Format(" -i {0} -i {1} -shortest {2} -y", @"C:\images\video.avi", @"C:\images\voice.wav", @"C:\images\result.avi")
            };

            using Process exeProcess = Process.Start(startInfo);
            
            string StdOutVideo = exeProcess.StandardOutput.ReadToEnd();
            string StdErrVideo = exeProcess.StandardError.ReadToEnd();
            exeProcess.WaitForExit();
            exeProcess.Close();
            
            var uploader = new YouTubeUploader();
            return "http://youtube.com/watch?v=" + uploader.Upload();
        }
    }

    public static class SoundInfo
    {
        [DllImport("winmm.dll")]
        private static extern uint mciSendString(
            string command,
            StringBuilder returnValue,
            int returnLength,
            IntPtr winHandle);

        public static int GetSoundLength(string fileName)
        {
            StringBuilder lengthBuf = new StringBuilder(32);

            mciSendString(string.Format("open \"{0}\" type waveaudio alias wave", fileName), null, 0, IntPtr.Zero);
            mciSendString("status wave length", lengthBuf, lengthBuf.Capacity, IntPtr.Zero);
            mciSendString("close wave", null, 0, IntPtr.Zero);

            int length = 0;
            int.TryParse(lengthBuf.ToString(), out length);

            return length;
        }
    }
}
