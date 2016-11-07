namespace CodeFirstPhonebook.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DropCreateDatabaseIfModelChanges<PhonebookContext>
    {
        protected override void Seed(PhonebookContext context)
        {
            var peter = new Contact
            {
                Name = "Peter Ivanov",
                Position = "CTO",
                Company = "Smart Ideas",
                Site = "http://blog.peter.com",
                Notes = "Friend from school"
            };
            peter.Emails.Add(new Email { EmailAddress = "peter@gmail.com" });
            peter.Emails.Add(new Email { EmailAddress = "peter_ivanov@yahoo.com" });

            peter.Phones.Add(new Phone { PhoneNumber = "+359 2 22 22 22" });
            peter.Phones.Add(new Phone { PhoneNumber = "+359 88 77 88 99" });

            var maria = new Contact
            {
                Name = "Maria"
            };
            maria.Phones.Add(new Phone { PhoneNumber = "+359 22 33 44 55" });

            var angie = new Contact
            {
                Name = "Angie Stanton",
                Site = "http://angiestanton.com"
            };
            angie.Emails.Add(new Email { EmailAddress = "info@angiestanton.com" });

            context.Contacts.Add(peter);
            context.Contacts.Add(maria);
            context.Contacts.Add(angie);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
