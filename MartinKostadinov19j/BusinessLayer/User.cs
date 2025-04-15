using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }
       
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        [Range(18, 81)]
        public int Age { get; set; }

        [Required]
        [StringLength(20)]

        public string UserName { get; set; }

        
        [Required]
        [StringLength(70)]

        public string Password { get; set; }
        
        [Required]
        [StringLength(20)]

        public string Email { get; set; }

        public List<User> Friends { get; set; }

        public List<Interest> Interests { get; set; }

        private User()
        {
            
        }
        public User(string firstName, string lastName, int age, string userName, string password, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            UserName = userName;
            Password = password;
            Email = email;
            Friends = new List<User>();
            Interests = new List<Interest>();
        }
    }
}
