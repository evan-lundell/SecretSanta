using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SecretSanta.Models
{
    public class ExceptionGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public ICollection<PersonExceptionGroup> PersonExceptionGroups { get; set; }

        public ExceptionGroup()
        {
            PersonExceptionGroups = new Collection<PersonExceptionGroup>();
        }
    }
}
