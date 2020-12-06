using FootballManager.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballManager.Data.Models
{
   public class Leagues : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Country { get; set; }
    }
}
