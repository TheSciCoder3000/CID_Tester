using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CID_Tester.Model.DTO
{
    public class TestPlanDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TEST_CODE { get; set; }
        public DateTime DATE { get; set; }
        public int CYCLE_NO { get; set; }
        public int TEST_TIME { get; set; }

        public DutDTO DUT { get; set; } = null!;

        public UserDTO TEST_USER { get; set; } = null!;

        public ICollection<ParameterDTO>? TEST_PARAMETERS { get; set; }
    }
}
