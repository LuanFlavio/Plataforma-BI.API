using System.Collections.Concurrent;
using Lamar;
using Microsoft.AspNetCore.Mvc;
using PlataformaBI.API.Services;

namespace PlataformaBI.API.Controllers
{
    public class GsoftController : ControllerBase
    {
        private const string HeaderTokenName = "gsoft-wd-token";

        private readonly ConcurrentDictionary<string, Session> sessions;

        public GsoftController(ConcurrentDictionary<string, Session> sessions)
        {
            this.sessions = sessions;
        }

        public bool UserAuthenticated
        {
            get
            {
                string st = this.GetToken();
                return this.sessions.Any(x => x.Key.Equals(st));
            }
        }

        public Session Session => this.sessions[this.GetToken()];

        public Container Container => this.Session.Container;

        private string GetToken()
        {
            if (!Request.Headers.Keys.Contains(HeaderTokenName))
                return string.Empty;

            return Request.Headers[HeaderTokenName];
        }
    }
}
