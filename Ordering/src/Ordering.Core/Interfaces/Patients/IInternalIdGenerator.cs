using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.Interfaces.Patients
{
    public interface IInternalIdGenerator
    {
        string NewInternalId();
    }
}