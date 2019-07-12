namespace SpotSet.Api.Models
{
    public class SetlistDto
    {
        public string id { get; set; }
        public string eventDate { get; set; }
        public Artist artist { get; set; }
        public Venue venue { get; set; }
        public Sets sets { get; set; }
    }
}