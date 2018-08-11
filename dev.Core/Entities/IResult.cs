using dev.Entities.Models;
using System.Collections.Generic;

namespace dev.Core.Entities
{
    public interface IResult
    {
        List<IModel> Data { get; set; }
        string Message { get; set; }
        bool Success { get; set; }
    }
}
