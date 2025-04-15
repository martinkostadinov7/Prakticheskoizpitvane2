using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Interest
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public List<User> Users { get; set; }
        
        public Field Field { get; set; }

        private Interest()
        {
            
        }
        public Interest(string name)
        {
            Name = name;
            Users = new List<User>();
        }
    }
}
