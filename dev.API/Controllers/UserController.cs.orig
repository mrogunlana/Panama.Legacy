using System.Web.Http;
using System.Collections;
using System.Linq;
using dev.Business.Commands;
using dev.Business.Validators;
using dev.Core.Commands;
using dev.Core.Entities;
using dev.Entities.Models;
using dev.Core.IoC;


namespace dev.Api.Controllers
{
    public class UserController : ApiController
    {
      
        public UserController()
        {

        }

        //[Authorize]
        [HttpGet]
        public string[] Echo()
        {
            var user = new dev.Entities.Models.User();
            user.LastName = "Smith";
            var handler = new Handler(null, ServiceLocator.Current);
            handler.Add(user);
            var result = handler.Validate<FirstNameNotNullOrEmpty>()
                                .Validate<EmailNotNullOrEmpty>()
                                .Invoke();
            return result.Messages.ToArray();
        }

<<<<<<< HEAD
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
=======
>>>>>>> 0d9d3c145859863a1a26b534f91937c777fb3309
    }
}
