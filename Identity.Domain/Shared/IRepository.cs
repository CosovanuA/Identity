using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain.Shared
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
