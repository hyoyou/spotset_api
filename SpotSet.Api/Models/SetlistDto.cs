namespace SpotSet.Api.Models
{
    public class SetlistDto
    {
        public string Id { get; set; }
        public string EventDate { get; set; }
        public Artist Artist { get; set; }
        public Venue Venue { get; set; }
        public Sets Sets { get; set; }
    }
}