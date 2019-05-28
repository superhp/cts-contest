using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtsContestWeb.Db.Entities
{
    public class Task : IAuditable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string InputType { get; set; }
        public string OutputType { get; set; }
        public int Value { get; set; }
        public bool Enabled { get; set; }
        public List<TaskTestCase> TestCases { get; set; }
        public DateTime Created { get; set; }
    }
}
