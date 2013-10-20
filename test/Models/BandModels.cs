using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace test.Models
{
    [Table("BandProfile")]
    public class BandProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BandId { get; set; }
        public string BandName { get; set; }
        public int Creator { get; set; }

        // todo
    }
}
