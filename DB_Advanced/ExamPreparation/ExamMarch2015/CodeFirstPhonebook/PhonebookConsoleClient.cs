using CodeFirstPhonebook.Models;
using CodeFirstPhonebook.Models.JsonModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstPhonebook
{
    public class PhonebookConsoleClient
    {
        static void Main(string[] args)
        {
            var ctx = new PhonebookContext();

            //ListContacts(ctx);

            //ImportContactsFromJson(ctx);
        }

        private static void ImportContactsFromJson(PhonebookContext ctx)
        {
            var file = File.ReadAllText("../../data/contacts.json");
            var jsonContacts = JsonConvert.DeserializeObject<JsonContact[]>(file);

            foreach (var contact in jsonContacts)
            {
                try
                {
                    ProcessContactInfo(contact, ctx);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void ProcessContactInfo(JsonContact contact, PhonebookContext ctx)
        {
            if (contact.Name != null)
            {
                var newContact = new Contact
                {
                    Name = contact.Name,
                    Position = contact.Position,
                    Company = contact.Company,
                    Site = contact.Site,
                    Notes = contact.Notes
                };

                if (contact.Phones != null)
                {
                    foreach (var phone in contact.Phones)
                    {
                        newContact.Phones.Add(new Phone { PhoneNumber = phone });
                    }
                }

                if (contact.Emails != null)
                {
                    foreach (var email in contact.Emails)
                    {
                        newContact.Emails.Add(new Email { EmailAddress = email });
                    }
                }

                ctx.Contacts.Add(newContact);
                ctx.SaveChanges();
                Console.WriteLine($"Contact {newContact.Name} imported");
            }
            else
            {
                throw new ArgumentException("Error: Name is required");
            }
        }

        private static void ListContacts(PhonebookContext ctx)
        {
            var contacts = ctx.Contacts.Select(c => new
            {
                c.Name,
                Emails = c.Emails.Select(e => e.EmailAddress),
                Phones = c.Phones.Select(p => p.PhoneNumber)
            })
            .ToList();

            contacts.ForEach(c =>
            {
                Console.WriteLine($"---{c.Name}");
                if (c.Phones.Any())
                {
                    Console.WriteLine($"Phones: {string.Join(", ", c.Phones)}");
                }

                if (c.Emails.Any())
                {
                    Console.WriteLine($"Emails: {string.Join(", ", c.Emails)}");
                }
            });
        }
    }
}
