namespace CodeFirstPhonebook
{
    using Migrations;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class PhonebookContext : DbContext
    {
        public PhonebookContext()
            : base("name=PhonebookContext")
        {
            Database.SetInitializer(new Configuration());
        }

        public IDbSet<Contact> Contacts { get; set; }

        public IDbSet<Email> Emails { get; set; }

        public IDbSet<Phone> Phones { get; set; }
    }
}