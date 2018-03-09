using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CtsContestWeb.Db.Entities
{
    public class ContactInfo : IAuditable
    {
        [Key]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string CourseName { get; set; }
        public string Degree { get; set; }
        public int? CourseNumber { get; set; }
        public string Answer { get; set; }
        //klausimas ar turetu implementuot IAuditable interfeisa
        public DateTime Created { get; set; }
    }
}
