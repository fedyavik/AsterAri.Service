namespace AsteriskAriService.Models.Asterisk
{
    /// <summary>
    /// Protocol specific additional parameter
    /// </summary>
    public class AdditionalParamModel
    {
        /// <summary>
        /// Name of the parameter
        /// </summary>
        public string Parameter_name { get; set; }
        
        /// <summary>
        /// Value of the parameter
        /// </summary>
        public string Parameter_value { get; set; }
    }
}