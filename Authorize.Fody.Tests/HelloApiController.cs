using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Autorize.Fody.Tests
{
    public class HelloApiController
    {
        [HttpGet]
        public string HelloGet()
        {
            return "Hello, world!";
        }

        [HttpPost]
        public string HelloPost()
        {
            return "Hello, world!";
        }

        [HttpPost]
        public HttpResponseMessage HelloMessage()
        {
            return new HttpResponseMessage { };
        }

        public string Hello()
        {
            return "Hello, world!";
        }
    }
}
