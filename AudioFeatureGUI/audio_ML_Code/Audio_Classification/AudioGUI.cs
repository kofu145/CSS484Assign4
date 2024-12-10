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
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using WMPLib;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

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
            
            exportJson();
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

        private void exportJson()
        {
            /*
             * Amplitude: ampMatrixMusic, ampMatrixSpeech (2d list of doubles)
             * Average Energy: aeMusic, aeSpeech (list of doubles)
             * Zero crossing rage: zcrMusic, zcrSpeech (List of doubles)
             * Frequency: frqMatrixMusic, frqMatrixSpeech (List<List<(double frequency, double magnitude)>>)
             * Spectroid: scMusic, scSpeech (list of doubles)
             */

            var export = new Dictionary<string, FeatureModel>();
            var csvString = "file_name,avg_energy,zero_cross_range,bandwidth,spectral_centroid,label" + Environment.NewLine;

            int count = 0;
            foreach (string fileName in musicAudioFiles)
            {
                FeatureModel featureModel = new FeatureModel();
                //featureModel.Amplitude = featureModel.RMS_Value(ampMatrixMusic[count]);
                featureModel.AverageEnergy = aeMusic[count];
                featureModel.ZeroCrossRange = zcrMusic[count];
                featureModel.Bandwidth = 
                    frqMatrixMusic[count].Where(e => e.frequency > 0).Max().frequency - 
                    frqMatrixMusic[count].Where(e => e.frequency > 3).Min().frequency;
                featureModel.SpectralCentroid = scMusic[count];
                featureModel.IsMusic = true;
                export[fileName] = featureModel;

                csvString += $"{fileName}," +
                             $"{featureModel.AverageEnergy}," +
                             $"{featureModel.ZeroCrossRange}," +
                             $"{featureModel.Bandwidth}," +
                             $"{featureModel.SpectralCentroid}," +
                             $"{featureModel.IsMusic}" + Environment.NewLine;
                count++;
            }

            count = 0;
            foreach (string fileName in speechAudioFiles)
            {
                FeatureModel featureModel = new FeatureModel();
                //featureModel.Amplitude = featureModel.RMS_Value(ampMatrixSpeech[count]);
                featureModel.AverageEnergy = aeSpeech[count];
                featureModel.ZeroCrossRange = zcrSpeech[count];
                featureModel.Bandwidth = 
                    frqMatrixSpeech[count].Where(e => e.frequency > 0).Max().frequency - 
                    frqMatrixSpeech[count].Where(e => e.frequency > 0).Min().frequency;
                featureModel.SpectralCentroid = scSpeech[count];
                featureModel.IsMusic = false;
                export[fileName] = featureModel;
                csvString += $"{fileName}," +
                             $"{featureModel.AverageEnergy}," +
                             $"{featureModel.ZeroCrossRange}," +
                             $"{featureModel.Bandwidth}," +
                             $"{featureModel.SpectralCentroid}," +
                             $"{featureModel.IsMusic}" + Environment.NewLine;
                count++;
            }
            
            string json = JsonConvert.SerializeObject(export, Formatting.Indented);

            File.WriteAllText("./export.json", json);
            File.WriteAllText("./export.csv", csvString);
        }
    }
}
