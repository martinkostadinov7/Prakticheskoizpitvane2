using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Field
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
    
        public List<User> Users { get; set; }

        private Field()
        {
            
        }
        public Field(string name)
        {
            Name = name;
            Users = new List<User>();
        }
    }
}
