﻿namespace Phonebook.Models.JsonModels
{
    using System;

    public class JsonMessage
    {
        public string Content { get; set; }

        public DateTime DateTime { get; set; }

        public string Recipient { get; set; }

        public string Sender { get; set; }
    }
}
