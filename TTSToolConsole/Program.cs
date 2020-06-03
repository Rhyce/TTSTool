using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace TTSTool
{
    class Program
    {
        static void Main(string[] args)
        {

            string TextToSpeak;

            // Initialize a new instance of the SpeechSynthesizer.  
            SpeechSynthesizer synth = new SpeechSynthesizer();

            List<InstalledVoice> installedVoices = synth.GetInstalledVoices().ToList();
            foreach(InstalledVoice voice in installedVoices)
            {
                Console.WriteLine(voice.VoiceInfo.Name);
            }

            Console.Write("Text to output: ");
            TextToSpeak = Console.ReadLine();
            synth.SelectVoice("Microsoft Eva Mobile");
            // Configure the audio output.   
            synth.SetOutputToDefaultAudioDevice();

            synth.Speak(TextToSpeak);
            synth.SetOutputToWaveFile($"{Console.ReadLine()}.wav");
            synth.Speak(TextToSpeak);

            synth.Dispose();

        }
    }
}
