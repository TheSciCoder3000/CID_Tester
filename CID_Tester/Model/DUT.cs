using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CID_Tester.Model
{
    public class DUT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DutCode { get; set; }

        public string DutName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public ICollection<TEST_PLAN> TEST_PLANS { get; set; } = null!;

    }
}
