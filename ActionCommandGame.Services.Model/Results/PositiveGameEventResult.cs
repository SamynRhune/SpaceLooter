using ActionCommandGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ActionCommandGame.Services.Model.Results
{
    public class PositiveGameEventResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Money { get; set; }
        public int Experience { get; set; }
        public int Probability { get; set; }
    }
}
