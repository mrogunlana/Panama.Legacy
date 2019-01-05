using dev.Business.Commands;
using dev.Business.Validators;
using dev.Entities.Models;
using Panama.Commands;
using Panama.Entities;
using Panama.IoC;
using System.Linq;
using System.Web.Http;

namespace dev.API.Controllers
{
    public class UserController : ApiController
    {
        public UserController() { }

        [HttpGet]
        public string[] Echo()
        {
            var result = new Handler(ServiceLocator.Current)
                .Add(new User()
                {
                    LastName = "Smith"
                })
                .Validate<FirstNameNotNullOrEmpty>()
                .Validate<EmailNotNullOrEmpty>()
                .Invoke();

            return result.Messages.ToArray();
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("user/save")]
        public IResult Save([FromBody]User user)
        {
            return new Handler(ServiceLocator.Current)
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
            return new Handler(ServiceLocator.Current)
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
