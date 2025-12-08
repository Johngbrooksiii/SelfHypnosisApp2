using HypnosisApp.Core.Services;
using System.Runtime.InteropServices;

namespace HypnosisApp.Core.Services;

public class FrequencyEngine : IFrequencyEngine
{
    private const int SampleRate = 44100;
    private const short MaxValue = short.MaxValue;

    // Placeholder: Generates a raw PCM buffer (Requires audio library like NAudio or platform APIs for actual playback)
    public byte[] GenerateIsochronicTone(double carrierFreq, double pulseFreq, double durationSeconds)
    {
        int numSamples = (int)(SampleRate * durationSeconds);
        short[] data = new short[numSamples];
        
        for (int i = 0; i < numSamples; i++)
        {
            double t = (double)i / SampleRate;
            
            // Carrier Tone (The base frequency, e.g., 200Hz)
            double carrier = Math.Sin(2 * Math.PI * carrierFreq * t);
            
            // Modulator (The pulsing frequency, e.g., 10Hz)
            double modulator = Math.Sign(Math.Sin(2 * Math.PI * pulseFreq * t)); 
            
            // Isochronic Signal: Carrier pulsed ON/OFF by the modulator
            double signal = modulator * carrier * 0.5; // 0.5 for half volume

            data[i] = (short)(signal * MaxValue);
        }

        // Convert short array to byte array
        var bytes = new byte[data.Length * sizeof(short)];
        Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length);
        return bytes;
    }
}
