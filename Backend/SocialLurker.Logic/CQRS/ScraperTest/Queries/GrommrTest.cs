using FluentValidation;
using MediatR;
using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialLurker.Logic.CQRS.ScraperTest.Queries
{
    public abstract class GrommrTest
    {
        #region ------------------------------------------------------ Query ---------------------------------------------------------
        //----------- Query model -----------
        public class Query : IRequest<Result>
        {

        }

        //----------- Query example -----------
        //public class Example : IExampleProvider<Query>
        //{
        //    public Query GetExample()
        //    {
        //        return new Query()
        //        {

        //        };
        //    }
        //}

        //----------- Query validator -----------
        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                //Validation examples:
                //RuleFor(x => x.Property1).NotNull();
                //RuleFor(x => x.Property2).Length(0, 10);
                //RuleFor(x => x.Property3).EmailAddress();
                //RuleFor(x => x.Property4).InclusiveBetween(18, 60);
            }
        }
        #endregion

        #region ------------------------------------------------------ Result --------------------------------------------------------
        //----------- Query result model -----------
        public class Result
        {
            public string ResponsePayload { get; set; }
        }
        #endregion

        #region ------------------------------------------------------ Handler -------------------------------------------------------
        //----------- Query handler -----------
        public class Handler : IRequestHandler<Query, Result>
        {
            public Handler()
            {

            }

            public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
            {
                var extra = new PuppeteerExtra();
                extra.Use(new StealthPlugin());

                List<string> programmerLinks = new List<string>();

                var browser = await extra.LaunchAsync(new LaunchOptions()
                {
                    Headless = false,
                    ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
                    Product = Product.Chrome
                });
                var page = await browser.NewPageAsync();
                //await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 5.1; rv:5.0) Gecko/20100101 Firefox/5.0");
                await page.GoToAsync("https://www.grommr.com/Account/LogOn");

                var usernameElement = await page.WaitForSelectorAsync("input[name='UserName']");
                var passwordElement = await page.WaitForSelectorAsync("input[name='Password']");
                var buttonElement = await page.WaitForSelectorAsync("input[type='submit']");

                await usernameElement.FocusAsync();
                await usernameElement.TypeAsync("");

                await passwordElement.FocusAsync();
                await passwordElement.TypeAsync("");

                await buttonElement.ClickAsync();


                return await Task.FromResult(new Result() { 
                    ResponsePayload = string.Empty
                });
            }
        }
        #endregion
    }
}
