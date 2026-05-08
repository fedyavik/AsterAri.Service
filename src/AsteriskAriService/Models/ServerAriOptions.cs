using AsteriskAriService.Bridges;
using AsteriskAriService.DtmfHandlers;
using AsteriskAriService.Middlewares;
using Microsoft.Extensions.Configuration;

namespace AsteriskAriService.Models
{
    public class ServerAriOptions
    {
        public AriConnectOptions ConnectOptions { get; set; } = new();
        internal Dictionary<string, Type> StasisHandlers { get; } = new ();
        internal List<Type> Middlewares { get; } = new();
        internal List<Type> DtmfHandlers { get; } = new();
        internal List<Type> Bridges { get; } = new();
        
        public void AddStasisHandler<T>(string key) where T : StasisHandler
        {
            StasisHandlers[key] = typeof(T);
        }
        public void AddMiddleware<T>() where T : BaseAriMiddleware
        {
            Middlewares.Add(typeof(T));
        }
        public void AddDtmfHandler<T>() where T : BaseDtmfHandler
        {
            DtmfHandlers.Add(typeof(T));
        }
        public void AddBridge<T>() where T : BaseBridgeAri
        {
            Bridges.Add(typeof(T));
        }
    }

    public class AriConnectOptions
    {
        public AriConnectOptions(){}
        public AriConnectOptions(IConfigurationSection section)
        {
            Hostname = section.GetSection("Hostname").Get<string>()!;
            Port = section.GetSection("Port").Get<int>();
            UserName = section.GetSection("UserName").Get<string>()!;
            Password = section.GetSection("Password").Get<string>()!;
            AppName = section.GetSection("AppName").Get<string>()!;
            UseSsl = section.GetSection("UseSsl").Get<bool>();
        }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AppName { get; set; }
        public bool UseSsl { get; set; }
    }
}