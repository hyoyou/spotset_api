using System.Collections.Generic;

namespace SpotSet.Api.Models
{
    public class SpotSetDto
    {
        public string id { get; set; }
        public string eventDate { get; set; }
        public string artist { get; set; }
        public string venue { get; set; }
        public List<TracksDto> tracks { get; set; }
    }
}