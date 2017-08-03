using Microsoft.AspNetCore.Http;
using ExtCore.Data.Abstractions;

namespace Barebone
{
    public interface IRequestHandler
    {
        HttpContext HttpContext { get; }
        //IStorage Storage { get; }
    }
}