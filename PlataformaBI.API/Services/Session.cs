using Domain;
using DomainDependencyInjection;
using Lamar;
using System.Collections.Concurrent;
using System.Timers;
using Timer = System.Timers.Timer;

namespace PlataformaBI.API.Services
{
    public class Session : IDisposable
    {
        private const byte MinutesToCheckSession = 1;
        private const byte MinutesToDie = 255;
        private const ushort ConstMinuteToMilliSeconds = 60000;

        private readonly Usuarios user;
        private readonly ConcurrentDictionary<string, Session> sessions;
        private readonly Timer sessionTimer;
        private readonly string token;
        private readonly Container container;

        private DateTime lastRequest;

        public Session(ConcurrentDictionary<string, Session> sessions, Usuarios usuario)
            : this(sessions)
        {
            this.user = usuario;

            this.container = new Container(opt =>
            {
                opt.Include(DomainServiceRegister.GetRegister());
                opt.For<Session>().Use(this);
            });

            this.sessionTimer.Start();

            if (!sessions.TryAdd(this.token, this))
            {
                this.Dispose();
                throw new InvalidOperationException("Exception to add session on sessions");
            }
        }

        protected Session(ConcurrentDictionary<string, Session> sessions)
        {
            this.sessions = sessions;
            this.token = Guid.NewGuid().ToString();
            this.sessionTimer = new Timer(MinutesToCheckSession * ConstMinuteToMilliSeconds);
            this.lastRequest = DateTime.Now;
            this.sessionTimer.Elapsed += SessionTimeOutCheck;
        }

        public Usuarios usuarioLogado => this.user;

        public string Token => this.token;

        public DateTime LastRequest => this.lastRequest;

        public Container Container
        {
            get
            {
                this.UpdateLastRequest(DateTime.Now);

                return this.container;
            }
        }

        public void UpdateLastRequest(DateTime? date = null)
        {
            this.UpdateLastRequest(date ?? DateTime.Now);
        }

        public void Dispose()
        {
            this.sessions.Remove(this.token, out _);
            this.sessionTimer.Stop();
            this.sessionTimer.Dispose();
            this.container.Dispose();
        }

       private void UpdateLastRequest(DateTime date)
        {
            this.lastRequest = date;
        }

        private void SessionTimeOutCheck(object? sender, ElapsedEventArgs e)
        {
            if ((DateTime.Now - this.lastRequest).Minutes >= MinutesToDie)
                this.Dispose();
        }
    }
}
