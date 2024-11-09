using System.Collections.Generic;

namespace SmartMatrix.Core.BaseClasses.Web
{
    public interface IApiResult
    {
        List<string> Messages { get; set; }

        bool Succeeded { get; set; }
        int? StatusCode { get; set; }
    }

    public interface IApiResult<out T> : IApiResult
    {
        T Data { get; }
    }
}