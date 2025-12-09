using HypnosisApp.Core.Models;
using SQLite;
using System.Text.Json;

namespace HypnosisApp.UI.Data;

public class SessionRepository : ISessionRepository
{
    private readonly SQLiteAsyncConnection _database;
    private const string DatabaseFilename = "sessions.db3";

    public SessionRepository()
    {
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
        _database = new SQLiteAsyncConnection(databasePath);
        _database.CreateTableAsync<SessionLog>().Wait();
    }

    public async Task<List<SessionTemplate>> GetAllSessionsAsync()
    {
        var sessions = new List<SessionTemplate>();

        try
        {
            // Load session templates from Resources/Raw folder
            var assembly = typeof(SessionRepository).Assembly;
            var resourceNames = assembly.GetManifestResourceNames()
                .Where(name => name.Contains("sessions") && name.EndsWith(".json"));

            foreach (var resourceName in resourceNames)
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(resourceName);
                if (stream != null)
                {
                    var session = await JsonSerializer.DeserializeAsync<SessionTemplate>(stream);
                    if (session != null)
                    {
                        sessions.Add(session);
                    }
                }
            }

            // If no sessions found in resources, try loading from Raw folder
            if (sessions.Count == 0)
            {
                var stressReliefStream = await FileSystem.OpenAppPackageFileAsync("stress_relief.json");
                if (stressReliefStream != null)
                {
                    var session = await JsonSerializer.DeserializeAsync<SessionTemplate>(stressReliefStream);
                    if (session != null)
                    {
                        sessions.Add(session);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading sessions: {ex.Message}");
        }

        return sessions;
    }

    public async Task<SessionTemplate?> GetSessionByTitleAsync(string title)
    {
        var sessions = await GetAllSessionsAsync();
        return sessions.FirstOrDefault(s => s.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }

    public async Task LogSessionAsync(string sessionTitle, DateTime startTime, DateTime endTime, bool completed)
    {
        var log = new SessionLog
        {
            SessionTitle = sessionTitle,
            StartTime = startTime,
            EndTime = endTime,
            Completed = completed
        };

        await _database.InsertAsync(log);
    }

    public async Task<List<SessionLog>> GetSessionHistoryAsync(int limit = 50)
    {
        return await _database.Table<SessionLog>()
            .OrderByDescending(s => s.StartTime)
            .Take(limit)
            .ToListAsync();
    }
}
