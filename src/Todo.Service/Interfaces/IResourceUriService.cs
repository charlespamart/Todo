using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Domain.Interfaces
{
    public interface IResourceUriService
    {
        Uri GetResourceUri(Guid id);
    }
}
