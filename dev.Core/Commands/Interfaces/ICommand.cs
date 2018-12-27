using dev.Core.Entities;
using System.Collections.Generic;

namespace dev.Core.Commands
{
    public interface ICommand
    {
        void Execute(List<IModel> data);
    }
}
