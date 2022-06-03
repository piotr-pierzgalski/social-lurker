using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialLurker.Logic
{
    public abstract class QueryBoilerplate //Rename me!
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
            public IQueryable<object> ResponsePayload { get; set; }
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
                return await Task.FromResult(new Result());
            }
        }
        #endregion
    }
}
