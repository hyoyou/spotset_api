using System.Collections.Generic;

namespace SpotSet.Api.Models
{
    public class Set
    {
        public ICollection<Song> song { get; set; }
    }
}