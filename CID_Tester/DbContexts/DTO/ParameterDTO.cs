using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.DbContexts.DTO
{
    public class ParameterDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PARAM_CODE { get; set; }

        [Required]
        public TestPlanDTO TEST_PLAN { get; set; } = null!;


        public string? DESCRIPTION { get; set; }

        [Required]
        public string METRIC { get; set; } = null!;

        [Required]
        public decimal VALUE { get; set; }

        [Required]
        public decimal TARGET { get; set; }

        [Required]
        public int PASS { get; set; }
    }
}
