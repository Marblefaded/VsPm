using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

[Parallelizable(ParallelScope.Self)]
public class Tests : PageTest
{
    [Test]
    public async Task Clicking_ContactButton_Goes_To_ContactForm()
    {
        await Page.GotoAsync("http://localhost/pm");
        var formButton = Page.Locator("text=Open Contact Form");
        await formButton.ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex(".*Home/Form"));
    }
}