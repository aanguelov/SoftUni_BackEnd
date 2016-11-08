namespace Phonebook.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;

    internal sealed class Configuration : DropCreateDatabaseIfModelChanges<PhonebookContext>
    {
        protected override void Seed(PhonebookContext context)
        {

            var vlado = new User
            {
                Username = "VGeorgiev",
                FullName = "Vladimir Georgiev",
                Phone = "0894545454"
            };

            var nakov = new User
            {
                Username = "Nakov",
                FullName = "Svetlin Nakov",
                Phone = "0897878787"
            };

            var angel = new User
            {
                Username = "Ache",
                FullName = "Angel Georgiev",
                Phone = "0897121212"
            };

            var alex = new User
            {
                Username = "Alex",
                FullName = "Alexandra Svilarova",
                Phone = "0894151417"
            };

            var petya = new User
            {
                Username = "Petya",
                FullName = "Petya Grozdarska",
                Phone = "0895464646"
            };

            context.Users.Add(vlado);
            context.Users.Add(nakov);
            context.Users.Add(angel);
            context.Users.Add(alex);
            context.Users.Add(petya);

            var malinki = new Channel { Name = "Malinki" };
            var softuni = new Channel { Name = "SoftUni" };
            var admins = new Channel { Name = "Admins" };
            var prog = new Channel { Name = "Programmers" };
            var geeks = new Channel { Name = "Geeks" };

            context.Channels.Add(malinki);
            context.Channels.Add(softuni);
            context.Channels.Add(admins);
            context.Channels.Add(prog);
            context.Channels.Add(geeks);

            var chmessage1 = new ChannelMessage
            {
                Channel = malinki,
                Content = "Hey dudes, are you ready for tonight?",
                Date = DateTime.Now,
                User = petya
            };

            var chmessage2 = new ChannelMessage
            {
                Channel = malinki,
                Content = "Hey Petya, this is the SoftUni chat.",
                Date = DateTime.Now,
                User = vlado
            };

            var chmessage3 = new ChannelMessage
            {
                Channel = malinki,
                Content = "Hahaha, we are ready!",
                Date = DateTime.Now,
                User = nakov
            };

            var chmessage4 = new ChannelMessage
            {
                Channel = malinki,
                Content = "Oh my god. I mean for drinking beers!",
                Date = DateTime.Now,
                User = petya
            };

            var chmessage5 = new ChannelMessage
            {
                Channel = malinki,
                Content = "We are sure!",
                Date = DateTime.Now,
                User = vlado
            };

            context.ChannelMessages.Add(chmessage1);
            context.ChannelMessages.Add(chmessage2);
            context.ChannelMessages.Add(chmessage3);
            context.ChannelMessages.Add(chmessage4);
            context.ChannelMessages.Add(chmessage5);

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
