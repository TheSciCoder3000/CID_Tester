using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace CID_Tester.Model;

public class TEST_OUTPUT
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OutputCode { get; set; }

    [Required]
    public string Measured { get; set; } = null!;

    [Required]
    public int DutLocation { get; set; }
 
    [Required]
    public TEST_PARAMETER TEST_PARAMETER { get; set; } = null!;

    [Required]
    public TEST_BATCH TEST_BATCH { get; set; } = null!;

    // TODO: add parameter "PASS"
    // TODO: add parameter "Cycle"
}
