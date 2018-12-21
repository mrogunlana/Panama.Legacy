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
    }
}
