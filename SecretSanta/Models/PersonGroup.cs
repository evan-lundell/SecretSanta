using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SecretSanta.Models
{
    public class PersonGroup
    {
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
