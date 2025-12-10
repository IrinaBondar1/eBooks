using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NivelServicii
{
    public class TestService : ITestService
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

}
