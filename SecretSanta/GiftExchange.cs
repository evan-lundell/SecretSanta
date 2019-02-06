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
                throw new Exception("An Event has not been initialize yet. Either load an existing event or create a new one.");
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
            if (groupEvent == null)
            {
                throw new Exception("An Event has not been initialize yet. Either load an existing event or create a new one.");
            }

            foreach (GiftPair gp in groupEvent.GiftPairs)
            {
                var fromAddress = new MailAddress("evanpop3test@gmail.com", "Secret Santa");
                var fromPassword = "Control#1";
                var toAddress = new MailAddress(gp.Giver.Email);
                string subject = $"{groupEvent.Name} info for {gp.Giver.FirstName}";
                string body = $"<html><body><p>{gp.Giver.FirstName},</p><p><h1>You have {gp.Recipient.FirstName}!</h1></p><p>The wish list can be found here: https://docs.google.com/spreadsheets/d/11QSdf0OWxW2Ax2tGYnwm5eSDylRRvrkwrBw_0TSRdOk/edit</p>";
                body += "<p>If you notice an issue with this drawing (ex. you have yourself or a member of your immediate family), please let Evan know. Do not reply to this email, text or call Evan/Karla.</p><p>Thanks!</p></body></html>";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    smtp.Send(message);
                }
            }
        }

        public void EmailEventLeader()
        {
            if (groupEvent == null)
            {
                throw new Exception("An Event has not been initialize yet. Either load an existing event or create a new one.");
            }

            StringBuilder builder = new StringBuilder(@"<html><head><style>#gifts {
    font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif;
    border-collapse: collapse;
    width: 100%;
}

#gifts td, #gifts th {
    border: 1px solid #ddd;
    padding: 8px;
}

#gifts tr:nth-child(even){background-color: #f2f2f2;}

#gifts tr:hover {background-color: #ddd;}

#gifts th {
    padding-top: 12px;
    padding-bottom: 12px;
    text-align: left;
    background-color: #4CAF50;
    color: white;
}</style></head>");
            builder.Append("<body><table id='gifts'><tr><th>Gift Giver</th><th>Gift Recipient</th></tr>");
            foreach (GiftPair gp in groupEvent.GiftPairs)
            {
                builder.AppendLine($"<tr><td>{gp.Giver.FirstName}</td><td>{gp.Recipient.FirstName}</td></tr>");
            }
            builder.AppendLine("</table></div></body></html>");

            var fromAddress = new MailAddress("evanpop3test@gmail.com", "Secret Santa");
            var fromPassword = "Control#1";
            var toAddress = new MailAddress(groupEvent.LeaderEmail);
            string subject = $"{groupEvent.Name} Master List";
            string body = builder.ToString();
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress))
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
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

        public string PrintGiftPairs()
        {
            if (groupEvent == null)
            {
                throw new Exception("An Event has not been initialize yet. Either load an existing event or create a new one.");
            }

            StringBuilder builder = new StringBuilder();
            foreach (GiftPair groupPair in groupEvent.GiftPairs)
            {
                builder.AppendLine($"{groupPair.Giver.FirstName} -> {groupPair.Recipient.FirstName}");
            }

            return builder.ToString();
        }
    }
}
