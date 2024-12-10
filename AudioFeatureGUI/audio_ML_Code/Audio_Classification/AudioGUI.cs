using Microsoft.VisualBasic.ApplicationServices;
using NAudio.Gui;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Audio_Classification
{
    public partial class AudioGUI : Form
    {
        public AudioGUI()
        {
            InitializeComponent();
        }

        AudioFeatures features = new AudioFeatures();

        /*
         * Loads audio files and stores them onto a list as data source for listBox and function references
         */
        List<string> musicAudioFiles = new List<string>(Directory.GetFiles("Audio_Files/Music", "mu*.wav"));
        List<string> speechAudioFiles = new List<string>(Directory.GetFiles("Audio_Files/speech", "sp*.wav"));

        /*
         * Functions below help support GUI and allow users to play audio clips.
         */
        private void AudioGUI_Load(object sender, EventArgs e)
        {
            rBtnSpeech.Checked = true;

            obtainAmplitude(musicAudioFiles, speechAudioFiles);
            obtainTimeDomain(ampMatrixMusic, ampMatrixSpeech);
            
            obtainFrequency(musicAudioFiles, speechAudioFiles);
            obtainFreqDomain(frqMatrixMusic, frqMatrixSpeech);
        }

        /*
         * changes between filters and their respective files
         */
        private void rBtn_CheckedChanged(object sender, EventArgs e)
        {
            lstBox.Items.Clear();
            if (rBtnSpeech.Checked)
            {
                for (int i = 0; i < speechAudioFiles.Count; i++)
                {
                    string fileName = speechAudioFiles[i].Substring(19);
                    lstBox.Items.Add(fileName);
                }
            }
            else
            {
                for (int i = 0; i < musicAudioFiles.Count; i++)
                {
                    string fileName = musicAudioFiles[i].Substring(18);
                    lstBox.Items.Add(fileName);
                }
            }
        }

        /*
         * List box for selection of items, change of media player, and for function implementation
         */
        private void lstBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = lstBox.SelectedIndex;

                lblFileName.Text = "File Name: " + lstBox.SelectedItem.ToString();

                string file = "";

                if (rBtnMusic.Checked)
                    file = Path.GetFullPath(musicAudioFiles[index]);
                else
                    file = Path.GetFullPath(speechAudioFiles[index]);

                if (rBtnMusic.Checked)
                    waveViewer1.WaveStream = new NAudio.Wave.WaveFileReader(Path.GetFullPath(musicAudioFiles[index]));
                else
                    waveViewer1.WaveStream = new NAudio.Wave.WaveFileReader(Path.GetFullPath(speechAudioFiles[index]));

                MediaPlayer.URL = file;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        /*
         * Amplitude retrieval of audio files OG
         */
        public List<List<double>> ampMatrixMusic = new List<List<double>>();
        public List<List<double>> ampMatrixSpeech = new List<List<double>>();
        private void obtainAmplitude(List<string> musicFiles, List<string> speechFiles) //obtains amplitudes for each file
        {
            foreach (string fileName in musicFiles)
            {
                AudioFeatures features = new AudioFeatures();
                List <double> temp = features.GetAmplitude(fileName);
                ampMatrixMusic.Add(temp);
            }

            foreach (string fileName in speechFiles)
            {
                AudioFeatures features = new AudioFeatures();
                List<double> temp = features.GetAmplitude(fileName);
                ampMatrixSpeech.Add(temp);
            }
        }

        /*
         * obtaining frequency of audio files
         */
        public List<List<(double frequency, double magnitude)>> frqMatrixMusic = new List<List<(double frequency, double magnitude)>>();
        public List<List<(double frequency, double magnitude)>> frqMatrixSpeech = new List<List<(double frequency, double magnitude)>>();
        private void obtainFrequency(List<string> musicFiles, List<string> speechFiles)
        {
            foreach (string fileName in musicFiles)
            {
                List<(double frequency, double magnitude)> temp = features.GetFrequency(fileName);
                frqMatrixMusic.Add(temp);
            }

            foreach (string fileName in speechFiles)
            {
                List<(double frequency, double magnitude)> temp = features.GetFrequency(fileName);
                frqMatrixSpeech.Add(temp);
            }
        }

        /*
         * Performing spectral centroid with provided frequencies and magnitudes
         */
        List<double> scMusic = new List<double>();
        List<double> scSpeech = new List<double>();
        private void obtainFreqDomain(List<List<(double frequency, double magnitude)>> musicFiles, List<List<(double frequency, double magnitude)>> speechFiles)
        {
            // music file processing
            scMusic = features.spectralCentroid(musicFiles);

            // speech file processing
            scSpeech = features.spectralCentroid(speechFiles);
        }

        /*
         * Performing average energy and zero crossing rate functions with provided amplitudes. Store the list to
         */
        List<double> aeMusic = new List<double>();
        List<double> aeSpeech = new List<double>();
        
        List<double> zcrMusic = new List<double>();
        List<double> zcrSpeech = new List<double>();
        private void obtainTimeDomain(List <List <double>> musicFiles, List<List <double>> speechFiles)
        {
            // music file processing
            AudioFeatures features = new AudioFeatures();
            aeMusic = features.averageEnergy(musicFiles);
            zcrMusic = features.zeroCrossingRate(musicFiles);

            // speech file processing
            aeSpeech = features.averageEnergy(speechFiles);
            zcrSpeech = features.zeroCrossingRate(speechFiles);
        }
    }
}
