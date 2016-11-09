namespace Phonebook
{
    using Newtonsoft.Json;
    using Models;
    using Models.JsonModels;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Xml.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var ctx = new PhonebookContext();
            //ctx.Channels.Count();

            //ListChannelsWithMessages(ctx);

            //ImportMessagesFromJson(ctx);
        }

        private static void ImportMessagesFromJson(PhonebookContext ctx)
        {
            var file = File.ReadAllText("../../data/messages.json");

            var jsonMessages = JsonConvert.DeserializeObject<JsonMessage[]>(file);

            foreach (var message in jsonMessages)
            {
                try
                {
                    ProccessMessageInfo(message, ctx);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void ProccessMessageInfo(JsonMessage message, PhonebookContext ctx)
        {
            if (message.Content == null)
            {
                throw new ArgumentException("Error: Content is required");
            }

            if (message.DateTime == null)
            {
                throw new ArgumentException("Error: DateTime is required");
            }

            if (message.Recipient == null)
            {
                throw new ArgumentException("Error: Recipient is required");
            }

            if (message.Sender == null)
            {
                throw new ArgumentException("Error: Sender is required");
            }

            var recipient = ctx.Users.FirstOrDefault(u => u.Username == message.Recipient);

            if (recipient == null)
            {
                throw new ArgumentException("Error: Recipient not in database");
            }

            var sender = ctx.Users.FirstOrDefault(u => u.Username == message.Sender);

            if (sender == null)
            {
                throw new ArgumentException("Error: Sender not in database");
            }

            var newMessage = new UserMessage
            {
                Content = message.Content,
                Date = message.DateTime,
                Recipient = recipient,
                Sender = sender
            };

            ctx.UserMessages.Add(newMessage);
            ctx.SaveChanges();
            Console.WriteLine($"Message \"{newMessage.Content}\" imported");
        }

        private static void ListChannelsWithMessages(PhonebookContext ctx)
        {
            var channels = ctx.Channels.Select(ch => new
            {
                ch.Name,
                messages = ch.Messages.Select(m => new
                {
                    m.Content,
                    m.Date,
                    user = m.User.Username
                })
            })
            .ToList();

            channels.ForEach(ch =>
            {
                Console.WriteLine($"{ch.Name}{Environment.NewLine}");
                Console.WriteLine($"--Messages--{Environment.NewLine}");

                foreach (var message in ch.messages)
                {
                    Console.WriteLine($"Content: {message.Content}, DateTime: {message.Date}, User: {message.user}");
                }
                Console.WriteLine();
            });
        }
    }
}
