using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstPhonebook.Models.JsonModels
{
    public class JsonContact
    {
        public string Name { get; set; }

        public string Company { get; set; }

        public string Position { get; set; }

        public string Site { get; set; }

        public string Notes { get; set; }

        public string[] Emails { get; set; }

        public string[] Phones { get; set; }
    }
}
