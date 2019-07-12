using System.Collections.Generic;

namespace SpotSet.Api.Models
{
    public class SpotSetDto
    {
        public string Id { get; set; }
        public string EventDate { get; set; }
        public string Artist { get; set; }
        public string Venue { get; set; }
        public List<TracksDto> Tracks { get; set; }
    }
}