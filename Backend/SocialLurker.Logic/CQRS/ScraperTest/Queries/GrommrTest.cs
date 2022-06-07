using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;
using SocialLurker.Logic.ConfigurationMappings;
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
            public List<string> ResponsePayload { get; set; }
        }
        #endregion

        #region ------------------------------------------------------ Handler -------------------------------------------------------
        //----------- Query handler -----------
        public class Handler : IRequestHandler<Query, Result>
        {
            private IOptions<GrommrCredentials> _config;
            public Handler(IOptions<GrommrCredentials> config)
            {
                _config = config;
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
                await page.GoToAsync("https://www.grommr.com/Account/LogOn", 0);

                var usernameElement = await page.WaitForSelectorAsync("input[name='UserName']");
                var passwordElement = await page.WaitForSelectorAsync("input[name='Password']");
                var buttonElement = await page.WaitForSelectorAsync("input[type='submit']");

                await usernameElement.FocusAsync();
                await usernameElement.TypeAsync(_config.Value.Username);

                await passwordElement.FocusAsync();
                await passwordElement.TypeAsync(_config.Value.Password);

                await buttonElement.ClickAsync();
                await page.WaitForSelectorAsync("span[class='nav-text']");

                await page.GoToAsync("https://www.grommr.com/Member/Search#q=age%3D18%253B75%26bmi%3D16%253B70%26weight%3D80%253B600%26height%3D153%253B203%26username%3D%26locationName%3D%26location%3D%26imFollowing%3Dtrue");

                var listOfMembersElement = await page.WaitForSelectorAsync("div[class='listOfMembers']");

                var jsCode = @"() => {
                    var array = Array.from(document.querySelectorAll('div[class=""member-thumbnail""]'));
                    return array.map(element => { return {
                            avatarUrl: element.querySelector('img[class~=""presenceLine""]').src,
                            name: element.querySelector('div[class=""miniprofile""]').querySelector('a').innerText,
                            ageWeightHeight: element.querySelector('p').innerText
                        }
                    });
                }";

                var results = await listOfMembersElement.EvaluateFunctionAsync<GrommrMemberDto[]>(jsCode);

                //var listOfMemberElements = await page.QuerySelectorAllAsync("div[class='member-thumbnail']");
                //foreach (var memberElement in listOfMemberElements)
                //{
                //    var grommrMemberDto = new GrommrMemberDto();

                //    //var imageElement = await memberElement.QuerySelectorAsync("img[class='presenceLine']");
                //    //var jsSrc = await imageElement.GetPropertyAsync("src");
                //    //grommrMemberDto.AvatarUrl = (await jsSrc.JsonValueAsync()).ToString();

                //    //var ageWeightHeightElement = await memberElement.QuerySelectorAsync("span[title='Age, Height, Weight']");
                //    //var ageWeightHeightElementProperties = await ageWeightHeightElement.GetPropertiesAsync();
                //    //var jsAgeWeightHeightElement = (await ageWeightHeightElement.JsonValueAsync()).ToString();
                //    //var splitResult = jsAgeWeightHeightElement.Split('/');
                //    //grommrMemberDto.Age = int.Parse(splitResult[0].Trim());
                //    //if(splitResult.Count() > 1)
                //    //{
                //    //    grommrMemberDto.Height = double.Parse(splitResult[1].Trim());
                //    //}
                //    //if(splitResult.Count() > 2)
                //    //{
                //    //    grommrMemberDto.Weight = int.Parse(splitResult[2].Trim());
                //    //}

                //    //var miniprofileElement = await memberElement.QuerySelectorAsync("div[class='miniprofile']");
                //    //var nameElement = await memberElement.QuerySelectorAsync("a");
                //    //var jsName = await nameElement.GetPropertyAsync("innerText");
                //    //grommrMemberDto.Name = (await jsName.JsonValueAsync()).ToString();
                //}


                return await Task.FromResult(new Result() { 
                    ResponsePayload = results.Select(x => $"{x.Name} / {x.AvatarUrl} / {x.AgeWeightHeight}").ToList()
                });
            }

            private class GrommrMemberDto
            {
                public string AvatarUrl { get; set; }
                public string Name { get; set; }
                public string AgeWeightHeight { get; set; }
                //public string Age { get; set; }
                //public string Weight { get; set; }
                //public string Height { get; set; }
            }
        }
        #endregion
    }
}
