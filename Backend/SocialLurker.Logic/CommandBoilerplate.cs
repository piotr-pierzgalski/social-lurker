using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SocialLurker.Logic
{
    public abstract class CommandBoilerplate //Rename me!
    {
        #region ------------------------------------------------------ Command ------------------------------------------------------- 
        //----------- Command model -----------
        public class Command : IRequest<Result>
        {

        }

        //----------- Command example -----------
        //public class Example : IExampleProvider<Command>
        //{
        //    public Command GetExample()
        //    {
        //        return new Command()
        //        {

        //        };
        //    }
        //}

        //----------- Command validator -----------
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
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
        //----------- Command result model -----------
        public class Result
        {
            //Should usually be empty
        }
        #endregion

        #region ------------------------------------------------------ Handler -------------------------------------------------------
        //----------- Command handler -----------
        public class Handler : IRequestHandler<Command, Result>
        {
            public Handler()
            {

            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new Result());
            }
        }
        #endregion
    }
}
