using AsteriskAriService.Extensions;
using AsteriskAriService.Models;
using DemoApp.Dtmf;
using DemoApp.Middleware;
using DemoApp.Stasis;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*
 * Extensions.conf example setup
 *   exten => _x.,1,NoOp()
 *   same => n,Stasis(TestARI,"Call",${EXTEN})
 *   same => n,hangup()
 *
 * 
 * exten => 1,1,NoOp()
 *   same => n,Stasis(TestARI,"External")
 *   same => n,hangup()
 *
 * exten => 1,1,NoOp()
 *   same => n,Stasis(<AppName>,"<HandlerName>","<Parameter1>",...)
 *   same => n,hangup()
 */

builder.Services.AddServerAri(option =>
{
    option.ConnectOptions = new AriConnectOptions(config.GetSection("Asterisk"));
    
    option.AddStasisHandler<DefaultStasis>("Call");
    option.AddStasisHandler<ExternalStasis>("External"); 
    option.AddStasisHandler<TestDtmfStasis>("TestDtmf");

    option.AddDtmfHandler<TalkDtmfHandler>();
    
    //the order of addition is important
    option.AddMiddleware<ExceptionAriMiddleware>();
});


var app = builder.Build();
app.Run();
