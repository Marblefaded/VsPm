using MudBlazor;
using FluentAssertions;
using Vs.Pm.Web.Pages.TaskView.EditTask;
using Vs.Pm.Test.Extentions;
using Microsoft.AspNetCore.Components;
using static MudBlazor.CategoryTypes;
using Vs.Pm.Web.Data.ViewModel;
using Vs.Pm.Web.Pages.Project.EditProject;

namespace Vs.Pm.Test.Component
{
    [TestFixture]
    public class TestDialog: BunitTest
    {

        [Test]
        public async Task TestOpenClose()
        {
            var comp = Context.RenderComponent<MudDialogProvider>();
            comp.Markup.Trim().Should().BeEmpty();
            var service = Context.Services.GetService<IDialogService>() as DialogService;
            service.Should().NotBe(null);
            IDialogReference dialogReference = null;
            // open simple test dialog
            await comp.InvokeAsync(() => dialogReference = service?.Show<EditProject>());
            dialogReference.Should().NotBe(null);
            comp.Find("div.mud-dialog-container").Should().NotBe(null);
            // close by click outside the dialog
            comp.Find("div.mud-overlay").Click();
            comp.Markup.Trim().Should().BeEmpty();
            var result = await dialogReference.Result;
            result.Canceled.Should().BeTrue();
        }

        [Test]
        public async Task TestCloseButton()
        {
            var comp = Context.RenderComponent<MudDialogProvider>();
            comp.Markup.Trim().Should().BeEmpty();
            var service = Context.Services.GetService<IDialogService>() as DialogService;
            var dialogOptionsService = Context.Services.GetService<IDialogService>() as DialogService;
            service.Should().NotBe(null);
            IDialogReference dialogReference = null;
            // open simple test dialog
            var newModel = new ProjectViewModel();
            var options = new DialogOptions()
            {
                CloseButton = false,
                MaxWidth = MaxWidth.Medium
            };
            var parameters = new DialogParameters<EditProject> { { x => x.ProjectViewModel, newModel } };
            comp.InvokeAsync(() => dialogReference = service?.Show<EditProject>("", parameters, options));
            //close by click on cancel button
            comp.FindAll("button")[0].Click();
        }
    }
}
