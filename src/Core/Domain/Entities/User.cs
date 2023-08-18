using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseGuid
    {
        public User() { }
        public User(string userName, string firstName, string lastName)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
        }

        [Required]
        public string UserName { get; private set; }

        [Required]
        public string FirstName { get; private set; }

        [Required]
        public string LastName { get; private set; }
    }
}
