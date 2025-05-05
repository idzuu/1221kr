using Microsoft.Extensions.Options;
using System.Threading.Tasks;

public interface IVideoConferenceService
{
    Task<VideoConference> CreateConferenceAsync(string title, int durationMinutes);
}

public class ZoomConferenceService : IVideoConferenceService
{
    private readonly ZoomSettings _settings;

    public ZoomConferenceService(IOptions<ZoomSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<VideoConference> CreateConferenceAsync(string title, int durationMinutes)
    {
        // Реализация создания конференции в Zoom
        // Используйте Zoom API или другую платформу

        return new VideoConference
        {
            Id = "generated-id",
            JoinUrl = "https://zoom.us/j/generated-id",
            Password = "optional-password"
        };
    }
}

public class VideoConference
{
    public string Id { get; set; }
    public string JoinUrl { get; set; }
    public string Password { get; set; }
}
public class ZoomSettings
{
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public string DefaultPassword { get; set; }
    public int DefaultDurationMinutes { get; set; } = 60;
    public string SdkKey { get; set; }
    public string SdkSecret { get; set; }
}