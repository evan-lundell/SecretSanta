using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SecretSanta.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ICollection<PersonGroup> Groups { get; set; }
        public ICollection<PersonExceptionGroup> Exceptions { get; set; }
        public ICollection<GiftPair> GiverPairs { get; set; }
        public ICollection<GiftPair> RecipientPairs { get; set; }

        public Person()
        {
            Groups = new Collection<PersonGroup>();
            Exceptions = new Collection<PersonExceptionGroup>();
            GiverPairs = new Collection<GiftPair>();
            RecipientPairs = new Collection<GiftPair>();
        }
    }
}
