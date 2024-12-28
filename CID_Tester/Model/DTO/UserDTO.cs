﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CID_Tester.Model.DTO
{
    public class UserDTO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int USER_CODE { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FIRST_NAME { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LAST_NAME { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(150)")]
        public string EMAIL { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string PROFILE_IMAGE { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string USER_NAME { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string PASSWORD { get; set; } = null!;

        public ICollection<TestPlanDTO> TEST_PLANS { get; set; }

    }
}