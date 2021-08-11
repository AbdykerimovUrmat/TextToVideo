# TextToVideo

Test project, that allows to convert text to image, then image to synthesized voice and video, then merge audio and video together, at the end uploads result to YouTube

System.Drawing is used to create image <br>
System.Speech.Synthesis is used to create synthesized voice <br>
AForge is used, to create video from image <br>
ffmpeg is called using Process.Start(), and used to merge audio and video <br>
YouTube.API.v3 is used to upload video <br>




In order to be able to upload videos, you should go to https://console.cloud.google.com/, register there, add a project, enable YouTube Data API, add a credential (OAuth Client Id), then download 'user_secrets.json' file and put it in project folder

Credential creation process in detail (very important):

1. in 'OAuth consent screen' add your google mail to test users and add all YouTube scopes
2. in credential settings add http://localhost/authorize and http://127.0.0.1/authorize to 'Authorized redirect URIs'

download ffmpeg from https://www.ffmpeg.org/
AForge libs are included in project

links that saved me hours:

1. https://stackoverflow.com/questions/53584389/combine-audio-and-video-files-with-ffmpeg-in-c-sharp
2. https://www.codeproject.com/Questions/1100454/How-to-start-audio-and-video-capture-merge-audio-a
3. https://docs.microsoft.com/en-us/archive/msdn-magazine/2019/june/speech-text-to-speech-synthesis-in-net
4. https://stackoverflow.com/questions/16021302/c-sharp-save-text-to-speech-to-mp3-file
5. https://stackoverflow.com/questions/82319/how-can-i-determine-the-length-i-e-duration-of-a-wav-file-in-c
6. https://stackoverflow.com/questions/2070365/how-to-generate-an-image-from-text-on-fly-at-runtime
7. https://stackoverflow.com/questions/9744026/image-sequence-to-video-stream
8. https://www.youtube.com/watch?v=WDOupC5dyIQ
