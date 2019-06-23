using Ntsprkr_Assignment.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ntsprkr_Assignment.Models
{
    public class TargetAppVM
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name ="Health Check Endpoint URL")]
        public string HealthCheckUrl { get; set; }
        [Required]
        public int Interval { get; set; }
        public TargetApp ConvertToEntity()
        {
            return new TargetApp() { Id = string.IsNullOrEmpty(this.Id)? Guid.NewGuid(): Guid.Parse(this.Id), Name = this.Name, HealthCheckUrl = this.HealthCheckUrl, Interval = this.Interval };
        }
        public void ParseFromEntity(TargetApp entity)
        {

            Id = entity.Id.ToString();
            Name = entity.Name;
            HealthCheckUrl = entity.HealthCheckUrl;
            Interval = entity.Interval;
        }
    }
}
