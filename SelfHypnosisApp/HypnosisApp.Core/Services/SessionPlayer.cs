using HypnosisApp.Core.Models;

namespace HypnosisApp.Core.Services;

public class SessionPlayer : ISessionPlayer
{
    private readonly INarrationEngine _narrator;
    private readonly IFrequencyEngine _frequencyGen;

    public SessionPlayer(INarrationEngine narrator, IFrequencyEngine frequencyGen)
    {
        _narrator = narrator;
        _frequencyGen = frequencyGen;
    }

    public async Task PlaySessionAsync(SessionTemplate session)
    {
        foreach (var stage in session.Stages)
        {
            Console.WriteLine($"Starting Stage: {stage.StageName} at {stage.IsochronicHz}Hz");

            // 1. Generate and queue frequency audio data (needs platform-specific API call here)
            // var toneData = _frequencyGen.GenerateIsochronicTone(200, stage.IsochronicHz, stage.DurationSeconds);
            // PlatformAudioService.PlayBuffer(toneData); 
            
            // 2. Start Narration
            await _narrator.SpeakAsync(stage.NarrationText);

            // 3. Ensure stage duration is met
            await Task.Delay(TimeSpan.FromSeconds(stage.DurationSeconds));
        }
    }

    public void StopSession()
    {
        // TODO: Implement stopping all audio playback
    }
}
