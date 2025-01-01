using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CID_Tester.Model
{
    public class TEST_PLAN
    {
        public TEST_PLAN(string description, DateTime date, int cycleNo, int testTime)
        {
            Description = description;
            Date = date;
            CycleNo = cycleNo;
            TestTime = testTime;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestCode { get; private set; }
        public string Description { get; private set; }
        public DateTime Date { get; private set; }
        public int CycleNo { get; private set; }
        public int TestTime { get; private set; }

        public DUT DUT { get; set; } = null!;

        public TEST_USER TEST_USER { get; set; } = null!;

        public ICollection<TEST_PARAMETER> TEST_PARAMETERS { get; set; } = null!;



    }
}
