using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CID_Tester.Model
{
    public class DUT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DutCode { get; private set; }

        public string DutName { get; private set; } = null!;

        public string Description { get; private set; } = null!;

        public ICollection<TEST_PLAN> TEST_PLANS { get; set; } = null!;

        public DUT(int dutCode, string dutName, string description)
        {
            DutCode = dutCode;
            DutName = dutName;
            Description = description;
        }

    }
}
