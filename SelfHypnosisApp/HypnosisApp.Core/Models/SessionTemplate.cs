namespace HypnosisApp.Core.Models;

public enum FrequencyType { Alpha, Theta, Delta, Custom }

public class SessionStage
{
    public string StageName { get; set; }
    public string NarrationText { get; set; }
    public double DurationSeconds { get; set; }
    public FrequencyType TargetFrequency { get; set; }
    public double IsochronicHz { get; set; } // e.g., 10.0 Hz
    public string AmbientTrackFilename { get; set; }
}

public class SessionTemplate
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<SessionStage> Stages { get; set; } = new List<SessionStage>();
}
