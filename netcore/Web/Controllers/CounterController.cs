using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CounterController : ControllerBase
    {
        private static int _value = 0;
        [HttpGet]
        public int Get()
        {
            return _value;
        }

        [Route("increase")]
        [HttpPost]
        public int Increase()
        {
            return Interlocked.Increment(ref _value);
        }
        
        [Route("decrease")]
        [HttpPost]
        public int Decrease()
        {
            return Interlocked.Decrement(ref _value);
        }
    }
}