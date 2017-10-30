using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SecretSanta.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LeaderName { get; set; }
        public string LeaderEmail { get; set; }

        public ICollection<PersonGroup> PersonGroups { get; set; }

        public Group()
        {
            PersonGroups = new Collection<PersonGroup>();
        }
    }
}
