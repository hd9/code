using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Infrastructure.Cors
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class CorsApi : Attribute, ICorsPolicyProvider
    {

        private CorsPolicy _policy;
        private ITaConfiguration _config;


        public string Origins { get; set; }
        public string Headers { get; set; }
        public string Methods { get; set; }
        public bool SupportsCredentials { get; set; }

        public ITaConfiguration Config
        {
            get { return _config ?? (_config = DependencyResolver.Current.GetService<ITaConfiguration>()); }
            set { _config = value; }
        }




        public CorsPolicy Policy
        {
            get { return _policy ?? (_policy = CreatePolicy()); }
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Policy);
        }

        private CorsPolicy CreatePolicy()
        {
            var policy = new CorsPolicy();

            AddOrigins(Origins, policy);
            string keyValue = Config.GetAllowableOrigins();
            AddOrigins(keyValue, policy);
            AddHeaders(Headers, policy);
            AddMethods(Methods, policy);

            policy.SupportsCredentials = SupportsCredentials;

            return policy;
        }

        private void AddMethods(string key, CorsPolicy policy)
        {
            if (key == "*")
            {
                policy.AllowAnyMethod = true;
                return;
            }

            if (!string.IsNullOrWhiteSpace(key))
            {
                string[] methods = key.Split(';');
                foreach (string method in methods)
                {
                    policy.Methods.Add(method);
                }
            }
            policy.AllowAnyMethod = (!policy.Methods.Any());
        }

        private void AddHeaders(string key, CorsPolicy policy)
        {
            if (key == "*")
            {
                policy.AllowAnyHeader = true;
                return;
            }

            if (!string.IsNullOrWhiteSpace(key))
            {
                string[] headers = key.Split(';');
                foreach (string header in headers)
                {
                    policy.Headers.Add(header);
                }
            }
            policy.AllowAnyHeader = (!policy.Headers.Any());
        }

        private static void AddOrigins(string keyValue, CorsPolicy policy)
        {
            if (keyValue == "*")
            {
                policy.AllowAnyOrigin = true;
                return;
            }

            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                string[] domains = keyValue.Split(';');
                foreach (string domain in domains)
                {
                    policy.Origins.Add(domain);
                }
            }

            policy.AllowAnyOrigin = !(policy.Origins.Any());
        }
    }
}