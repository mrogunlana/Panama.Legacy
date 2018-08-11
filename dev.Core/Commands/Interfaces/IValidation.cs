using dev.Entities.Models;
using System.Collections.Generic;

namespace dev.Core.Commands
{
    public interface IValidation
    {
        bool IsValid(List<IModel> data);
        string Message();
    }
}
