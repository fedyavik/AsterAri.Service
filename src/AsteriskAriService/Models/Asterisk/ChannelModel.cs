
namespace AsteriskAriService.Models.Asterisk
{
    public class ChannelModel
    {
        /// <summary>
        /// Unique identifier of the channel.
        /// This is the same as the Uniqueid field in AMI.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the channel (i.e. SIP/foo-0000a7e3)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// allowableValues: ["Down","Rsrved","OffHook","Dialing","Ring","Ringing","Up","Busy","Dialing Offhook","Pre-ring","Unknown"]
        /// </summary>
        public string State { get; set; }

        public CallerIdModel Caller { get; set; }

        public CallerIdModel Connected { get; set; }

        public string Accountcode { get; set; }

        /// <summary>
        /// Current location in the dialplan
        /// </summary>
        public DialplanCEPModel Dialplan { get; set; }

        /// <summary>
        /// Timestamp when channel was created
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// The default spoken language
        /// </summary>
        public string Language { get; set; }
    }
}