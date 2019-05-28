using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CtsContestWeb.Db.Entities
{
    public class CodeSkeleton
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }
        public string Language { get; set; }
        public string WriteLine { get; set; }
        public string ReadLine { get; set; }
        public string ReadInteger { get; set; }
        public string ReadLineOfIntegers { get; set; }
        public string ReadInputIntegerNumberOfLinesOfIntegers { get; set; }
    }
}
