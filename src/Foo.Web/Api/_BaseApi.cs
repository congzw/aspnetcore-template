using System;
using Microsoft.AspNetCore.Mvc;

namespace Foo.Web.Api
{
    public interface IFooApi
    {
    }
    
    [ApiController]
    public abstract class BaseFooApi : IFooApi
    {
        [HttpGet("GetDesc")]
        public string GetDesc()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " GetDesc FROM " + this.GetType().Name;
        }
    }
}
