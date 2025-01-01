using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CID_Tester.Model
{
    public class TEST_PARAMETER
    {
        public TEST_PARAMETER(string name, string description, string metric, decimal value, decimal target, int pass, string parameters)
        {
            Name = name;
            Description = description;
            Metric = metric;
            Value = value;
            Target = target;
            Pass = pass;
            Parameters = parameters;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParamCode { get; private set; }

        [Required]
        public string Name { get; private set; }

        [Required]
        public TEST_PLAN TestPlan { get; set; } = null!;


        public string Description { get; private set; }

        [Required]
        public string Metric { get; private set; } = null!;

        [Required]
        public decimal Value { get; private set; }

        [Required]
        public decimal Target { get; private set; }

        [Required]
        public int Pass { get; private set; }

        [Required]
        public string Parameters { get; private set; }

    }
}
