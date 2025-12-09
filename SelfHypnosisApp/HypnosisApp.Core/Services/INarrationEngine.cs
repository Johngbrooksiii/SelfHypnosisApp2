namespace HypnosisApp.Core.Services;

public interface INarrationEngine
{
    // Uses Text-to-Speech (TTS) to read the script text
    Task SpeakAsync(string text, float speed = 0.8f);
    
    // Stops any currently playing narration
    void Stop();
}
