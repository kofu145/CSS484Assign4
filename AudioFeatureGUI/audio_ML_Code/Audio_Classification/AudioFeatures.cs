using NAudio.Wave;
using AForge.Math;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.Xml;
using NAudio.Dsp;


namespace Audio_Classification
{
    internal class AudioFeatures
    {
        public AudioFeatures() {  }

        /*
        * Extracts frequency of files
        */        
        public List<(double frequency, double magnitude)> GetFrequency(string filePath)
        {
            List<(double frequency, double magnitude)> frqMag = new List<(double frequency, double magnitude)>();

            using (var reader = new AudioFileReader(filePath))
            {
                // Get the sample rate of the audio file
                int sampleRate = reader.WaveFormat.SampleRate;

                // Calculate the number of samples in the audio file
                int totalSamples = (int)(reader.Length / (reader.WaveFormat.BitsPerSample / 8)); // Convert bytes to samples
                float[] sampleBuffer = new float[totalSamples];

                // Read the audio samples into the buffer
                int samplesRead = reader.Read(sampleBuffer, 0, totalSamples);


                double logLength = Math.Ceiling(Math.Log(samplesRead, 2.0));
                int paddedLength = (int)Math.Pow(2.0, Math.Min(Math.Max(1.0, logLength), 14.0)); // Ensures power of two

                // Create a complex array to hold the FFT input data
                AForge.Math.Complex[] complexData = new AForge.Math.Complex[paddedLength];

                // Copy valid audio samples into the complex array
                for (int i = 0; i < samplesRead; i++)
                {
                    if (i < complexData.Length)
                        complexData[i] = new AForge.Math.Complex(sampleBuffer[i], 0); // Real part = sample, Imaginary part = 0
                }


                // Perform the FFT using AForge.NET's FFT class
                AForge.Math.FourierTransform.FFT(complexData, AForge.Math.FourierTransform.Direction.Forward);

                // Calculate the magnitude for each frequency bin and corresponding frequencies
                for (int i = 0; i < complexData.Length / 2; i++)
                {
                    double magnitude = Math.Sqrt(Math.Pow(complexData[i].Re, 2) + Math.Pow(complexData[i].Im, 2));
                    double frequency = (i * sampleRate) / (double)paddedLength;
                    frqMag.Add((frequency, magnitude));
                }
            }

            return frqMag;
        }
        
        /*
         * Frequency Domain Feature
         */
        public List<double> spectralCentroid(List<List<(double frequency, double magnitude)>> freqMag)
        {
            List<double> spectralCentroid = new List<double>();

            for (int i = 0; i < freqMag.Count; i++) 
            {
                double sum = 0;
                double divisor = 0;
                for (int j = 0; j < freqMag[i].Count; j++)
                {
                    sum += (freqMag[i][j].frequency * freqMag[i][j].magnitude); //summation of magnitudes times frequencies
                    divisor += (freqMag[i][j].magnitude); //summation of magnitudes
                }

                spectralCentroid.Add(sum / divisor); // adds output value of file into the list
            }

            return spectralCentroid;
        }

        /*
         * Extracts Amplitude for each file OG
         */
        public List<double> GetAmplitude(string filePath)
        {
            List<double> amplitudes = new List<double>();

            using (var reader = new AudioFileReader(filePath))
            {
                // Create a buffer to store the samples
                var sampleBuffer = new float[reader.WaveFormat.SampleRate]; // 1 second worth of samples

                // Loop through the file to read and process its samples
                int samplesRead;
                while ((samplesRead = reader.Read(sampleBuffer, 0, sampleBuffer.Length)) > 0)
                {
                    for (int i = 0; i < samplesRead; i++)
                    {
                        // Get the amplitude of the sample and convert to double
                        double amplitude = sampleBuffer[i];
                        amplitudes.Add(amplitude);
                    }
                }
            }

            // Return the list of amplitudes
            return amplitudes;
        }
        
        /*
         * Time Domain features
         */
        public List<double> averageEnergy(List<List<double>> amplitude)
        {
            List <double> averageEnergy = new List<double>();

            for (int i = 0; i < amplitude.Count; i++)
            {
                double sum = 0;
                for (int j = 0;  j < amplitude[i].Count; j++)
                {
                    sum += Math.Pow(amplitude[i][j], 2); // follows the formula x(n)^2 / n
                }
                double value = sum / amplitude[i].Count;  // uses the sum and divides by total samples
                averageEnergy.Add(value);
            }

            return averageEnergy; //returns list of average energies for the files
        }
        
        public List<double> zeroCrossingRate(List<List<double>> amplitude)
        {
            List<double> zeroCrossingRate = new List<double>();

            for (int i = 0; i < amplitude.Count; i++)
            {
                double sum = 0;
                for (int j = 0; j < amplitude[i].Count; j++)
                {
                    if (j == 0)
                    {
                        continue;
                    }
                    sum += Math.Abs(zeroCrossingHelper(amplitude[i][j]) - zeroCrossingHelper(amplitude[i][j-1])); // follows the formula x(n)^2 / n
                }
                double value = sum / (2*(amplitude[i].Count - 1));  //uses the sum and divides by total samples
                zeroCrossingRate.Add(value);
            }

            return zeroCrossingRate; //returns list of average energies for the files
        }
        
        //helps determine the sign for zero crossing method
        private double zeroCrossingHelper(double value)
        {
            if (value > 0)
                return 1;
            if (value < 0)
                return -1;
            return value;
        }
    }
}
