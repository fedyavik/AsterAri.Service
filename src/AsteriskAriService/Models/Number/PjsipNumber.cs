namespace AsteriskAriService.Models.Number
{
    public class PjsipNumber : AriNumber
    {
        private string? Trunk { get; set; }

        public PjsipNumber(string number,string? trunk = null) : base(number)
        {
            Trunk = trunk;
            Endpoint = trunk is null ? $"PJSIP/{Number}" : $"PJSIP/{Number}@{trunk}";
        }
    }
}