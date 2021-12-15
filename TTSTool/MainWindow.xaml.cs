using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Speech.Synthesis;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace TTSTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static SpeechSynthesizer synth = new SpeechSynthesizer();

        static List<InstalledVoice> installedVoices = synth.GetInstalledVoices().ToList();

        static InstalledVoice selectedVoice;

        static string DirectoryPath;

        static string OutputFileName;

        static string FullOutputPath;
        public MainWindow()
        {
            InitializeComponent();

            PopulateVoiceDropdown();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        void PopulateVoiceDropdown()
        {
            foreach (InstalledVoice v in installedVoices)
            {
                VoiceDropdown.Items.Add(v.VoiceInfo.Name);
            }
            VoiceDropdown.SelectedIndex = 0;
        }

        private void Save_BTN_Click(object sender, RoutedEventArgs e)
        {
            SetOutputPath();

            synth.SelectVoice(installedVoices[VoiceDropdown.SelectedIndex].VoiceInfo.Name);
            synth.SetOutputToWaveFile(SetOutputPath());
            synth.Speak(TextToSpeak.Text);
        }

        private void VoiceDropdown_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            selectedVoice = installedVoices[VoiceDropdown.SelectedIndex];
        }


        string SetOutputPath()
        {
            if(OutputName.Text == "")
            {
                OutputName.Text = $@"TTS_{DateTime.Now.ToString("yyyyMMddTHHmmss")}";
                OutputFileName = OutputName.Text;
            }
            else
            {
                OutputFileName = OutputName.Text;
            }

            if (OutputFolder.Text == "")
            {
                OutputFolder.Text = Directory.GetCurrentDirectory();
                DirectoryPath = OutputFolder.Text;
            }
            else
            {
                DirectoryPath = OutputFolder.Text;
            }

            FullOutputPath = $@"{DirectoryPath}\{OutputFileName}.wav";

            CurrentOutputDisplayText.Text = FullOutputPath;
            return FullOutputPath;
        }

        private void OutputName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetOutputPath();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "Select Target Folder";
            dlg.IsFolderPicker = true;
            
            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = Directory.GetCurrentDirectory();
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                OutputFolder.Text = dlg.FileName;

                SetOutputPath();
            }
        }
    }
}
