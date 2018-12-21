using dev.Business.Validators;
using dev.Core.Commands;
using dev.Core.IoC;
using System.Linq;
using System.Web.Http;

namespace dev.Api.Controllers
{
    public class UserController : ApiController
    {
        public UserController() { }

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

    }
}