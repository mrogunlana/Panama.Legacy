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

    }
}
