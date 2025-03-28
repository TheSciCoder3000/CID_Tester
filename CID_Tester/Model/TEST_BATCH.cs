﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CID_Tester.Model
{
    public class TEST_BATCH
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BatchCode { get; set; }

        [Required]
        public int CycleNo { get; set; }

        [Required]
        public int TestTime { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TEST_PLAN TEST_PLAN { get; set; } = null!;

        [Required]
        public TEST_USER TEST_USER { get; set; } = null!;

        public ICollection<TEST_OUTPUT> TEST_OUTPUTS { get; set; } = null!;

        // TODO: add parameter "NumberOfDuts"
    }
}
