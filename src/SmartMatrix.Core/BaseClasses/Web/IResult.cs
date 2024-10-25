using System.Collections.Generic;

namespace SmartMatrix.Core.BaseClasses.web
{
    public interface IResult
    {
        List<string> Messages { get; set; }

        bool Succeeded { get; set; }
        int? StatusCode { get; set; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}