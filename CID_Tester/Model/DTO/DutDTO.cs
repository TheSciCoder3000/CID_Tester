using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CID_Tester.Model.DTO
{
    public class DutDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DUT_CODE { get; set; }

        public string DUT_NAME { get; set; } = null!;

        public string DESCRIPTION { get; set; } = null!;

        public ICollection<TestPlanDTO>? TEST_PLANS { get; set; }
    }
}
