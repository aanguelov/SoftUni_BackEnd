using MiniORM.Attributes;
using System;

namespace MiniORM.Entities
{
    [Entity("Users")]
    public class User
    {
        [Id]
        private int id;

        [Column("Username")]
        private string username;

        [Column("Password")]
        private string password;

        [Column("Age")]
        private int age;

        [Column("RegistrationDate")]
        private DateTime registrationDate;

        public User(string username, string password, int age, DateTime registrationDate)
        {
            this.Username = username;
            this.Password = password;
            this.Age = age;
            this.RegistrationDate = registrationDate;
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public int Age { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
