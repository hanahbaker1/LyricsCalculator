using System;

namespace LyricsCalculator.Api.Configuration
{
    public class AppConfig : IAppConfig
    {
        public Uri MusicBrainzSongUri { get; set; }
        public Uri OvhLyricsUri { get; set; }
    }
}
