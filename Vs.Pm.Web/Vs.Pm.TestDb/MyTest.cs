using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vs.Pm.TestDb.Extensions;
using Vs.Pm.Web.Data.Service;

namespace Vs.Pm.TestDb
{
    public class MyTests : IClassFixture<InjectionFixture>
    {
        private readonly InjectionFixture injection;
        public MyTests(InjectionFixture injection)
        {
            this.injection = injection;
            
        }

        [Fact]
        public void SomeTest()
        {
            
        }
    }
}
