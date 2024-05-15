
using ActionCommandGame.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ActionCommandGame.Services.Model.Requests
{
    public class PositiveGameEventRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Money { get; set; }
        public int Experience { get; set; }
        public int Probability { get; set; }


    }
}

