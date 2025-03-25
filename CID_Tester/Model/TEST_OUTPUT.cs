using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID_Tester.Model;

public class TEST_OUTPUT
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OutputCode { get; set; }

    [Required]
    public Double Measured { get; set; }

    [Required]
    public int DutNum { get; set; }

    [Required]
    public bool pass { get; set; }

    public TEST_PARAMETER TEST_PARAMETER { get; set; } = null!;
    public TEST_BATCH TEST_BATCH { get; set; } = null!;
}
