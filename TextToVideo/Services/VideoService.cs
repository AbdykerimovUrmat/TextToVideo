using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using AForge.Video.FFMPEG;

namespace Uploader.Services
{
    public class VideoService
    {
        public void CreateVideo(Image image, string path, string videoName, int frameRate, int lengthInSeconds)
        {
            using var videoWriter = new VideoFileWriter();
            videoWriter.Open(path + videoName, image.Width, image.Height, frameRate, VideoCodec.MPEG4, 1000000);

            for (int s = 0; s < lengthInSeconds; s++)
            {
                for (int f = 0; f < frameRate; f++)
                {
                    videoWriter.WriteVideoFrame(image as Bitmap);
                }
            }

            videoWriter.Close();
        }

        public void CreateVideoFromList(IEnumerable<(Image image, float duration)> images, int width, int height, string path, int frameRate)
        {
            using var videoWriter = new VideoFileWriter();
            videoWriter.Open(path, width, height, frameRate, VideoCodec.MPEG4, 1000000);

            foreach(var image in images)
            {
                for (int f = 0; f < frameRate * image.duration; f++)
                {
                    videoWriter.WriteVideoFrame(image.image as Bitmap);
                }
            }

            videoWriter.Close();
        }

        public void MergeVideoAudio(string videoPath, string audioPath, string resultPath)
        {
            string strCmdText;
            ProcessStartInfo startInfo = new()
            {
                CreateNoWindow = false,
                FileName = @"C:\Users\User\Documents\ffmpeg.exe",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = string.Format(" -i {0} -i {1} -shortest {2} -y", videoPath, audioPath, resultPath)
            };
            using Process exeProcess = Process.Start(startInfo);
            string StdOutVideo = exeProcess.StandardOutput.ReadToEnd();
            string StdErrVideo = exeProcess.StandardError.ReadToEnd();
            exeProcess.WaitForExit();
            exeProcess.Close();
        }
    }
}
