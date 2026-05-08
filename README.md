# AsterkAri.Service

#### REST + WebSocket client for working with Asterisk ARI (Stasis)

The library is designed to develop applications based on Stasis ARI using Dependency Injection and architecture approaches ASP.NET Core (.NET 8).

It allows you to conveniently manage Asterisk entities and build a full-fledged call processing logic.

### Opportunities
- Working via REST + WebSocket
- Integration with DI (ASP.NET Core)
- Management:
  - Channels
  - Bridges
  - Playbacks
  - Recordings
  - endpoints
  - Apps
- Call processing pipeline support
- Lifecycle management via scope sessions
- Parsing Asterisk events only if someone is subscribed to this event
- Using the http connection pool

### Session Lifecycle

- During the Stasis Start event:
  - a scope (DI scope) is being created, a separate call session
  - passing through the specified middlewares
- During the session:
  - events are being processed
  - resources (channels, bridges, etc.) are managed.
- At the end of the session:
  - all resources are being released
  - all channels connected to the bridge are terminated

```
Stasis Enter
    ↓
Create Scope (DI)
    ↓
Middlewares
    ↓
Stasis Handler / Call Logic
    ↓
Bridges
```

### Redirect
When redirecting from stasis to stasis:
 - the current session is ending
 - a new session is being created
 - shared data is saved between sessions

⚠️ The last session may NOT be completed if your logic does not handle the client shutdown event.
You may need this behavior, but then you should avoid using shared session resources.

## Usage example

`appsettings.json`:
```json
{
  "Asterisk": {
    "Hostname": "192.168.1.230",
    "Port": 8088,
    "AppName": "TestARI",
    "UserName": "asterisk",
    "Password": "asterisk",
    "UseSsl": false
  }
}
```

`program.cs`:
```csharp

builder.Services.AddServerAri(option =>
{
    option.ConnectOptions = new AriConnectOptions(config.GetSection("Asterisk"));
    
    option.AddStasisHandler<DefaultStasis>("Call");
    option.AddStasisHandler<ExternalStasis>("ExternalStasis"); 
    option.AddStasisHandler<TestDtmfStasis>("TestDtmf");

    option.AddDtmfHandler<TalkDtmfHandler>();
    
    //the order of addition is important
    option.AddMiddleware<ExceptionAriMiddleware>();
});

```

call processing logic for `ExternalStasis`:
```csharp

public override async Task Handler()
{
    ClientChannel = clientSessionAri.Initiator;
    logger.LogInformation("A new call from a client {Number} {ChannelId}", ClientChannel.PhoneNumber, ClientChannel.Id);
    
    await IntroStep();
    AnswerChannel = await CallStep();
    await TalkStep();
}

/// <summary>
/// Playing the intro to the client
/// </summary>
private async Task IntroStep()
{
    var clientHelloSound = "sound:en/hello";
    var soundState = await bridgeFactory.CreateBridgeAsync<BridgeSound>();
    await soundState.Handler(clientHelloSound);
}

/// <summary>
/// We are calling all managers.
/// </summary>
/// <returns>Returns the responding manager</returns>
private async Task<ChannelAri> CallStep()
{
    var number1 = new PjsipNumber("3333");
    var number2 = new PjsipNumber("3334");
    var ringState = await bridgeFactory.CreateBridgeAsync<BridgeRing>();
    var ringModel = new BridgeRingModel(ClientChannel,[number1, number2]);
    return await ringState.Handler(ringModel);
}

/// <summary>
/// The client's conversation with the manager
/// </summary>
/// <returns>Returns when the conversation is over</returns>
private async Task TalkStep()
{
    TalkState = await bridgeFactory.CreateBridgeAsync<BridgeTalk>();
    var model = new BridgeTalkModel(ClientChannel, AnswerChannel!, true);
    await TalkState.Handler(model);
}

```

See the full example in the [DemoApp](samples/DemoApp/Program.cs)