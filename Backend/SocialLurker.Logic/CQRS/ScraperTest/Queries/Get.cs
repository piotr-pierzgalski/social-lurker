using FluentValidation;
using MediatR;
using PuppeteerSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialLurker.Logic.CQRS.ScraperTest.Queries
{
    public abstract class Get
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
            public List<string> ResponsePayload { get; set; }
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
                string fullUrl = "https://en.wikipedia.org/wiki/List_of_programmers";

                List<string> programmerLinks = new List<string>();

                var options = new LaunchOptions()
                {
                    Headless = true,
                    ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
                    Product = Product.Chrome
                };
                //var browser = await Puppeteer.LaunchAsync(options, null, Product.Chrome);
                var browser = await Puppeteer.LaunchAsync(options);
                var page = await browser.NewPageAsync();
                await page.GoToAsync(fullUrl);
                var links = @"Array.from(document.querySelectorAll('a')).map(a => a.href);";
                var urls = await page.EvaluateExpressionAsync<string[]>(links);


                foreach (string url in urls)
                {
                    programmerLinks.Add(url);
                }

                return await Task.FromResult(new Result() { 
                    ResponsePayload = programmerLinks
                });
            }
        }
        #endregion
    }
}
