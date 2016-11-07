using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFirstPhonebook.Models
{
    public class Phone
    {
        public int Id { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
