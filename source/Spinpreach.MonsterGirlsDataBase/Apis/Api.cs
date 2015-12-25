using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spinpreach.MonsterGirlsDataBase.Apis
{
    public class Api
    {

        public Request request { get; set; } = new Request();
        public Response response { get; set; } = new Response();

        public class Request
        {
            //public Requests._home home { get; set; }
            //public Requests._login_start login_start { get; set; }
            //public Requests._conquest_complete conquest_complete { get; set; }
            //public Requests._conquest_start conquest_start { get; set; }
            //public Requests._conquest_cancel conquest_cancel { get; set; }
        }

        public class Response
        {
            //public Responses._home home { get; set; }
            //public Responses._login_start login_start { get; set; }
            //public Responses._conquest_complete conquest_complete { get; set; }
            //public Responses._conquest_start conquest_start { get; set; }
            //public Responses._conquest_cancel conquest_cancel { get; set; }
        }

    }
}