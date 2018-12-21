using dev.Entities.Models;
using System.Collections.Generic;

namespace dev.Core.Entities
{
    public interface IResult
    {
        List<IModel> Data { get; set; }
        IEnumerable<string> Messages { get; }
        void AddMessage(string message);
        bool Success { get; set; }
    }
}
