using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.HashiCorpVault;
using System.Collections.Generic;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly VaultOptions _options;
        private readonly IConfiguration _configuration;

        public ValuesController(VaultOptions vaultOptions, IConfiguration configuration)
        {
            _options = vaultOptions;
            _configuration = configuration;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<KeyValuePair<string, string>>> Get()
        {
            var result = new Dictionary<string, string>();

            foreach (var item in _options.Secrets)
            {
                var val = _configuration[item];
                result.Add(item, val);
            }
        
            return result;
        }

    }
}
