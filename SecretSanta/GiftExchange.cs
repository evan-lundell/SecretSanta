using Microsoft.EntityFrameworkCore;
using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SecretSanta
{
    public class GiftExchange
    {
        public static Random random = new Random();

        private readonly SecretSantaDbContext context;
        private Group group;
        private Event groupEvent;

        public GiftExchange(SecretSantaDbContext context)
        {
            this.context = context;
        }

        public void Initialize(string groupName)
        {
            group = context.Groups
                .Include(g => g.ExceptionGroups)
                .ThenInclude(eg => eg.PersonExceptionGroups)
                .ThenInclude(peg => peg.Person)
                .Include(g => g.PersonGroups)
                .ThenInclude(pg => pg.Person)
                .Include(g => g.Events)
                .ThenInclude(e => e.GiftPairs)
                .FirstOrDefault(g => g.Name.Equals(groupName, StringComparison.CurrentCultureIgnoreCase));
            if (group == null)
            {
                throw new Exception($"Group {groupName} not found.");
            }
        }

        public void CreateNewEvent(string eventName, string leaderName, string leaderEmail)
        {
            groupEvent = new Event
            {
                Name = eventName,
                LeaderName = leaderName,
                LeaderEmail = leaderEmail,
                GroupId = group.Id
            };
            context.Events.Add(groupEvent);
        }

        public void LoadExistingEvent(string eventName)
        {
            groupEvent = group.Events.FirstOrDefault(e => e.Name.Equals(eventName, StringComparison.CurrentCultureIgnoreCase));
            if (groupEvent == null)
            {
                throw new Exception($"Event {eventName} not found for Group {group.Name}.");
            }
        }

        public void GenerateGiftPairs()
        {
            if (groupEvent == null)
            {
                throw new Exception("An Event has been initialize yet. Either load an existing event or create a new one.");
            }

            groupEvent.GiftPairs.Clear();
            List<Person> persons = group.PersonGroups.Select(pg => pg.Person).ToList();
            List<Person> availableRecipients = persons.ToList();
            List<GiftPair> giftPairs = new List<GiftPair>();
            while (persons.Count > 0)
            {
                Person giver = persons[0];
                var validRecipients = availableRecipients.Where(r => giver.Id != r.Id && IsAllowedGiftPair(giver, r)).ToList();
                if (validRecipients.Count == 0)
                {
                    // Swap with someone else
                    var swappable = giftPairs.Where(gp => gp.RecipientId != giver.Id && IsAllowedGiftPair(giver, gp.Recipient)).ToList();
                    var swap = swappable[random.Next(swappable.Count)];
                    persons.Add(swap.Giver);
                    swap.Giver = giver;
                }
                else
                {
                    Person recipient = validRecipients[random.Next(validRecipients.Count)];
                    GiftPair pair = new GiftPair
                    {
                        EventId = groupEvent.Id,
                        Giver = giver,
                        Recipient = recipient
                    };
                    giftPairs.Add(pair);
                    availableRecipients.Remove(recipient);
                }
                persons.Remove(giver);
            }

            context.GiftPairs.AddRange(giftPairs);
            context.SaveChanges();
        }

        public void EmailGiftPairs()
        {

        }

        public void EmailEventLeader()
        {

        }

        private bool IsAllowedGiftPair(Person giver, Person recipient)
        {
            foreach (ExceptionGroup exceptionGroup in group.ExceptionGroups.Where(eg => eg.PersonExceptionGroups.Any(peg => peg.PersonId == giver.Id)))
            {
                if (exceptionGroup.PersonExceptionGroups.Any(peg => peg.PersonId == recipient.Id))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
