using HypnosisApp.Core.Models;

namespace HypnosisApp.Core.Services;

public class SessionPlayer : ISessionPlayer
{
    private readonly INarrationEngine _narrator;
    private readonly IFrequencyEngine _frequencyGen;
    private readonly IAudioPlaybackService _audioService;
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _isPlaying;

    public SessionPlayer(
        INarrationEngine narrator, 
        IFrequencyEngine frequencyGen,
        IAudioPlaybackService audioService)
    {
        _narrator = narrator;
        _frequencyGen = frequencyGen;
        _audioService = audioService;
    }

    public async Task PlaySessionAsync(SessionTemplate session)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _isPlaying = true;

        try
        {
            foreach (var stage in session.Stages)
            {
                if (_cancellationTokenSource.Token.IsCancellationRequested)
                    break;

                Console.WriteLine($"Starting Stage: {stage.StageName} at {stage.IsochronicHz}Hz");

                // 1. Speak the narration first
                if (!string.IsNullOrWhiteSpace(stage.NarrationText))
                {
                    await _narrator.SpeakAsync(stage.NarrationText);
                    
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                        break;
                }

                // 2. Generate and play isochronic tone for the stage duration
                // Using 200 Hz carrier frequency as specified in design docs
                var toneData = _frequencyGen.GenerateIsochronicTone(
                    carrierFreq: 200.0,
                    pulseFreq: stage.IsochronicHz,
                    durationSeconds: stage.DurationSeconds);

                // Play the tone (this will block until complete or cancelled)
                await _audioService.PlayPCMBufferAsync(
                    toneData, 
                    sampleRate: 44100, 
                    _cancellationTokenSource.Token);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Session cancelled by user");
        }
        finally
        {
            _isPlaying = false;
        }
    }

    public void StopSession()
    {
        if (!_isPlaying)
            return;

        Console.WriteLine("Stopping session...");
        
        // Cancel the playback
        _cancellationTokenSource?.Cancel();
        
        // Stop narration
        _narrator.Stop();
        
        // Stop all audio playback
        _audioService.StopAllAudioAsync().Wait();
        
        _isPlaying = false;
    }
}
