using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CID_Tester.Service.Serial;
using System.Diagnostics;
using System.Windows.Input;

namespace CID_Tester.Model
{
    public class TEST_PLAN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DUT DUT { get; set; } = null!;

        public ICollection<TEST_PARAMETER> TEST_PARAMETERS { get; set; } = [];
        public ICollection<TEST_BATCH> TEST_BATCHES { get; set; } = [];

    }
}
