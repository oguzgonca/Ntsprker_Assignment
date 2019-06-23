using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ntsprkr_Assignment.Data.Entities
{
    public class TargetApp
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(120)]
        public string HealthCheckUrl { get; set; }
        public int Interval { get; set; }
        public bool IsDown { get; set; }

    }
}
