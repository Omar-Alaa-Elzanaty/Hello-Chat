using Hello.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Hello.Presentation.EndPoints
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected Task<ActionResult<Result<T>>> HandleResponse<T>(Result<T> result)
        {
            return Task.FromResult<ActionResult<Result<T>>>(StatusCode((int)result.StatusCode, result));
        }
    }
}
