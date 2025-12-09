namespace HypnosisApp.UI.Services;

/// <summary>
/// Platform-specific audio playback service for mixing and playing audio streams
/// </summary>
public interface IAudioPlaybackService
{
    /// <summary>
    /// Plays raw PCM audio data (used for isochronic tones)
    /// </summary>
    Task PlayPCMBufferAsync(byte[] pcmData, int sampleRate, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Stops all currently playing audio
    /// </summary>
    Task StopAllAudioAsync();
    
    /// <summary>
    /// Sets the master volume for all audio playback
    /// </summary>
    void SetMasterVolume(float volume);
    
    /// <summary>
    /// Checks if audio is currently playing
    /// </summary>
    bool IsPlaying { get; }
}
