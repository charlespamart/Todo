using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using Todo.Domain.Interfaces;

namespace Todo.Domain.Services
{
    public class ResourceUriService : IResourceUriService
    {
        private LinkGenerator LinkGenerator;
        public ResourceUriService(LinkGenerator linkGenerator)
        {
            LinkGenerator = linkGenerator;
        }

        public Uri GetResourceUri(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
