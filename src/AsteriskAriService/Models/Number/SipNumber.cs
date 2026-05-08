namespace AsteriskAriService.Models.Number
{
    public class SipNumber : AriNumber
    {
        private string? Trunk { get; set; }

        public SipNumber(string number,string? trunk = null) : base(number)
        {
            Trunk = trunk;
            Endpoint = trunk is null ? $"SIP/{Number}" : $"SIP/{trunk}/{Number}";
        }
    }
}