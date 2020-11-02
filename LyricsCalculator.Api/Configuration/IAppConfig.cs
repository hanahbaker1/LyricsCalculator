using System;

namespace LyricsCalculator.Api.Configuration
{
    public interface IAppConfig
    {
        Uri MusicBrainzSongUri { get; set; }
        Uri OvhLyricsUri { get; set; }
    }
}