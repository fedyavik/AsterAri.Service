using AsteriskAriService.Actions.Bridge;
using AsteriskAriService.Bridges;
using AsteriskAriService.Extensions;
using AsteriskAriService.Models.Ari;
using Microsoft.Extensions.DependencyInjection;

namespace AsteriskAriService.Factories.Recordings
{
    public class RecordingFactory (IServiceProvider sp,
        IBridgeAriActions bridgeAriActions
        ) : IRecordingFactory
    {
        
        public async Task<LiveRecordingAri> StartRecordingAri(BaseBridgeAri bridge, ChannelAri channel)
        {
            var datetime = DateTime.Now;
            var recordingName = $"{GetType().Name}-0001-{channel.PhoneNumber}-" +
                                $"{datetime.GetDateAsterisk()}-{datetime.GetTimeAsterisk()}-{channel.Id}";
            var rec = await bridgeAriActions.RecordAsync(bridge.BridgeId, recordingName, "wav", ifExists: "overwrite", beep: false);
            
            var ariModel = sp.GetRequiredService<LiveRecordingAri>();
            ariModel.Init(rec);
            await channel.SetVariable("CDR(recordingfile)", ariModel.RecordingName);
            return ariModel;
        }
    }
}