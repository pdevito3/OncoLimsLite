namespace Ordering.WebApi.Features.Patients.Services
{
    using Microsoft.EntityFrameworkCore;
    using Ordering.Core.Entities;
    using Ordering.Core.Interfaces.Patients;
    using Ordering.Infrastructure.Contexts;
    using System;
    using System.Threading.Tasks;

    public class InternalIdGenerator : IInternalIdGenerator
    {
        public string NewInternalId()
        {
            //Could be some calculation service, incrementor, etc.
            return Guid.NewGuid().ToString();
        }
    }
}