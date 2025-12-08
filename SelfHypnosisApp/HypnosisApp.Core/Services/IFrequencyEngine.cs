namespace HypnosisApp.Core.Services;

public interface IFrequencyEngine
{
    // Generates raw PCM audio data for a given entrainment frequency
    byte[] GenerateIsochronicTone(double carrierFreq, double pulseFreq, double durationSeconds);
}
