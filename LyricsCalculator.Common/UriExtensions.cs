using System;

namespace LyricsCalculator.Common
{
    public static class UriExtensions
    {
        public static Uri GuaranteeEndingSlash(this Uri source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.AbsoluteUri.EndsWith("/")
                ? source
                : new Uri($"{source.AbsoluteUri}/");
        }
    }
}