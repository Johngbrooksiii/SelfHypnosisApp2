using HypnosisApp.Core.Models;

namespace HypnosisApp.Core.Services;

public interface ISessionPlayer
{
    // Orchestrates the session: mixing narration, tones, and ambient sounds
    Task PlaySessionAsync(SessionTemplate session);
    void StopSession();
}
