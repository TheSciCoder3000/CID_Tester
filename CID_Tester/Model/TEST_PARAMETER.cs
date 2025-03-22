﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CID_Tester.Model
{
    public class TEST_PARAMETER
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParamCode { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;

        [Required]
        public TEST_PLAN TestPlan { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string Metric { get; set; } = null!;

        public decimal? Value { get; set; }

        [Required]
        public decimal Target { get; set; }

        public bool? Pass { get; set; }

        [Required]
        public string Parameters { get; set; } = null!;

        [Required]
        public string InputConfiguration { get; set; } = null!;

        public Dictionary<string, bool> ParseToParameterDictionary()
        {
            Dictionary<string, bool> ParameterDictionary = new Dictionary<string, bool>();
            string[] parametersArray = Parameters.Split(", ");
            foreach (var parameter in parametersArray)
            {
                var relayStringSplit = parameter.Split('=');
                ParameterDictionary.Add(relayStringSplit[0], relayStringSplit[1] == "True");
            }
            return ParameterDictionary;
        }

    }
}
