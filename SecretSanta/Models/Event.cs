using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SecretSanta.Models
{
    public class Event
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public string Name { get; set; }

        public DateTime? Date { get; set; }

        public string LeaderName { get; set; }

        public string LeaderEmail { get; set; }

        public ICollection<GiftPair> GiftPairs { get; set; }

        public Event()
        {
            GiftPairs = new Collection<GiftPair>();
        }
    }
}
