using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Models
{
    public class GiftPair
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public int GiverId { get; set; }
        public Person Giver { get; set; }

        public int RecipientId { get; set; }
        public Person Recipient { get; set; }
    }
}
