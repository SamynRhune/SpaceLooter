
using ActionCommandGame.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ActionCommandGame.Services.Model.Requests
{
    public class NegativeGameEventRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefenseWithGearDescription { get; set; }
        public string DefenseWithoutGearDescription { get; set; }
        public int DefenseLoss { get; set; }
        public int Probability { get; set; }

    }
}

