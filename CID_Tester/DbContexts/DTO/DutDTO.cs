using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.DbContexts.DTO
{
    public class DutDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DUT_CODE { get; set; }

        public string? DESCRIPTION { get; set; }

        public ICollection<TestPlanDTO>? TEST_PLANS { get; set; }
    }
}
