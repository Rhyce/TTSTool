using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

            int i = 0;
            foreach(InstalledVoice voice in installedVoices)
            {
                Console.WriteLine($"{i}) {voice.VoiceInfo.Name}    |    {voice.VoiceInfo.Gender}");
                i++;
            }
            Console.Write("Select voice: ");
            int SelectedVoice = int.Parse(Console.ReadLine());
            Console.WriteLine($"Selected voice {installedVoices[SelectedVoice].VoiceInfo.Name}");
            Console.Write("Text to output: ");
            
            TextToSpeak = Console.ReadLine();
            synth.SelectVoice(installedVoices[SelectedVoice].VoiceInfo.Name);
            // Configure the audio output.   
            synth.SetOutputToWaveFile($@"{Directory.GetCurrentDirectory()}\{TextToSpeak}.wav");
            synth.Speak(TextToSpeak);
            Console.WriteLine($@"Output file to: {Directory.GetCurrentDirectory()}");
            
            synth.Dispose();

        }
    }
}
