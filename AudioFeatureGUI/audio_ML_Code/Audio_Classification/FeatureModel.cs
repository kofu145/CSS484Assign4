namespace Audio_Classification;

public class FeatureModel
{
    /*
     * Amplitude: ampMatrixMusic, ampMatrixSpeech (2d list of doubles)
     * Average Energy: aeMusic, aeSpeech (list of doubles)
     * Zero crossing rage: (List of doubles)
     * Frequency: frqMatrixMusic, frqMatrixSpeech (List<List<(double frequency, double magnitude)>>)
     * Spectroid: scMusic, scSpeech (list of doubles)
     */

    public double Amplitude;
    public double AverageEnergy;
    public double ZeroCrossRange;
    public double Bandwidth;
    public double SpectralCentroid;
    public bool IsMusic;
    
    // Root Mean Square  
    public double RMS_Value(List<double> arr)
    {
        int n = arr.Count;
        int square = 0;
        double mean, root = 0;            
 
        // Calculate square
        for (int i = 0; i < n; i++)
        {
            square += (int)Math.Pow(arr[i], 2);
        }
 
        // Calculate Mean
        mean = (square / (double)(n));
 
        // Calculate Root
        root = (double)Math.Sqrt(mean);
 
        return root;
    }
}

public class FrequencyModel
{
    public double Frequency;
    public double Magnitude;
}