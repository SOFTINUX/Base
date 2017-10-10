using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure
{
    /// <summary>
    /// Interface to allow access to things we need to access from anywhere in application such as http context and storage context,
    /// and that are injected to controllers. This interface limits the acess of modules to only these controllers' properties.
    /// </summary>
    public interface IRequestHandler
    {
        HttpContext HttpContext { get; }
        IStorage Storage { get; }
    }
}