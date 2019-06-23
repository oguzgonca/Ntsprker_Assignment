using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ntsprkr_Assignment.Models
{
    public interface IHealthCheckService
    {
        Task<bool> HealthCheckAsync(string url, Func<HttpResponseMessage, bool> responsePredicate);
    }

    public class HealthCheckService : IHealthCheckService
    {
        public async Task<bool> HealthCheckAsync(string url, Func<HttpResponseMessage,bool> responsePredicate)
        {
            using (var httpClient = new HttpClient())
            {
                var result = await httpClient.GetAsync(url);
                return responsePredicate(result);
            }
        }
    }
}
