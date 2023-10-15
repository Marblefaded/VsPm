using Microsoft.AspNetCore.Components;
using MudBlazor;
using Vs.Pm.Test.Extentions;

namespace Vs.Pm.Test
{
    public abstract class BunitTest
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        protected Bunit.TestContext Context { get; private set; }

        [SetUp]
        public virtual void Setup()
        {
            Context = new();
            Context.AddTestServices();
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                Context.Dispose();
            }
            catch (Exception)
            {
                /*ignore*/
            }
        }

      
        protected async Task ImproveChanceOfSuccess(Func<Task> testAction)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    await testAction();
                    return;
                }
                catch (Exception) { /*we don't care here*/ }
            }
            await testAction();
        }
    }
}