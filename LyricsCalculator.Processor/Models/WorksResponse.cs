﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LyricsCalculator.Processor.Models
{
    public class WorksResponse
    {
        public IEnumerable<Work> Works { get; set; }
        [JsonPropertyName("work-count")]
        public int WorkCount { get; set; }
    }
}