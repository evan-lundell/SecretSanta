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

        public ICollection<PersonGroup> PersonGroups { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<ExceptionGroup> ExceptionGroups { get; set; }

        public Group()
        {
            PersonGroups = new Collection<PersonGroup>();
            Events = new Collection<Event>();
            ExceptionGroups = new Collection<ExceptionGroup>();
        }
    }
}
