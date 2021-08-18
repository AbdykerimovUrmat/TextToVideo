
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace Uploader.Services
{
    public class AudioService
    {
        public void TextToAudio(string resultAudioPath, string text)
        {
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToWaveFile(resultAudioPath);
            synthesizer.Speak(text);
            synthesizer.Dispose();
        }

        public void ListToAudio(string resultAudioPath, IEnumerable<string> texts)
        {
            var synthesizer = new SpeechSynthesizer();
            synthesizer.SetOutputToWaveFile(resultAudioPath);
            foreach (var t in texts)
            {
                synthesizer.Speak(t);
            }
            synthesizer.Dispose();
        }
    }
}
