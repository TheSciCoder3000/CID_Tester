using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.DbContexts.DTO
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
