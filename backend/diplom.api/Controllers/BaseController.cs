using diplom.api.DataAccessLayer;
using diplom.api.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace diplom.api.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IDataAccessAdapter DataAccessAdapter;
        protected readonly IMemoryCache Cache;
        protected readonly AppSettings Settings;

        public BaseController(IDataAccessAdapter dataAccessAdapter, IMemoryCache cache, IOptions<AppSettings> settings)
        {
            this.Settings = settings.Value;
            this.Cache = cache;
            this.DataAccessAdapter = dataAccessAdapter;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
