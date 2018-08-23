using dev.Business.Commands;
using dev.Business.Validators;
using dev.Core.Commands;
using dev.Core.Entities;
using dev.Entities.Models;
using System.Web.Http;

namespace dev.Api.Controllers
{
    public class UserController : ApiController
    {
        private IHandler _handler;

        public UserController(IHandler handler)
        {
            _handler = handler;
        }

        [Authorize]
        [HttpGet]
        public string Echo()
        {
            return System.DateTime.Now.ToShortDateString();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("user/save")]
        public IResult Save([FromBody]User user)
        {
            return _handler
                .Add(user)
                .Validate<FirstNameNotNullOrEmpty>()
                .Validate<EmailNotNullOrEmpty>()
                .Validate<PasswordNotNullOrEmpty>()
                .Validate<ConfirmPasswordNotNullOrEmpty>()
                .Validate<PasswordAndConfirmPasswordMustMatch>()
                .Validate<EmailNotExist>()
                .Command<GenerateUserId>()
                .Command<HashUserPassword>()
                .Command<SaveUser>()
                .Invoke();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("user/update")]
        public IResult Update([FromBody]User user)
        {
            return _handler
                .Add(user)
                .Validate<FirstNameNotNullOrEmpty>()
                .Validate<EmailNotNullOrEmpty>()
                .Validate<PasswordNotNullOrEmpty>()
                .Validate<ConfirmPasswordNotNullOrEmpty>()
                .Validate<PasswordAndConfirmPasswordMustMatch>()
                .Validate<EmailNotExist>()
                .Command<GenerateUserId>()
                .Command<HashUserPassword>()
                .Command<SaveUser>()
                .Invoke();
        }
    }
}
