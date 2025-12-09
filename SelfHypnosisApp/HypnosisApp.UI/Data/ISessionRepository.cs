using HypnosisApp.Core.Models;

namespace HypnosisApp.UI.Data;

/// <summary>
/// Repository for managing session templates and session logs
/// </summary>
public interface ISessionRepository
{
    /// <summary>
    /// Loads all available session templates
    /// </summary>
    Task<List<SessionTemplate>> GetAllSessionsAsync();
    
    /// <summary>
    /// Loads a specific session template by title
    /// </summary>
    Task<SessionTemplate?> GetSessionByTitleAsync(string title);
    
    /// <summary>
    /// Logs a completed session
    /// </summary>
    Task LogSessionAsync(string sessionTitle, DateTime startTime, DateTime endTime, bool completed);
    
    /// <summary>
    /// Gets session history
    /// </summary>
    Task<List<SessionLog>> GetSessionHistoryAsync(int limit = 50);
}
