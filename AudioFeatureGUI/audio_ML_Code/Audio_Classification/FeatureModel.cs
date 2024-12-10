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

    public List<double> Amplitude;
    public double AverageEnergy;
    public double ZeroCrossRange;
    public List<FrequencyModel> FrequencyMatrix;
    public double SpectralCentroid;
    public bool IsMusic;
}

public class FrequencyModel
{
    public double Frequency;
    public double Magnitude;
}