using System.Net;

namespace OpenElectricity.Sdk.Models
{
    /// <summary>
    /// Error message when API returns <see cref="HttpStatusCode.UnprocessableContent"/>
    /// </summary>
    public class Error422Response
    {
        /// <summary>
        /// Detail
        /// </summary>
        public ErrorDetail? Detail { get; set; }
    }

    /// <summary>
    /// Error details in <see cref="Error422Response"/>
    /// </summary>
    public class ErrorDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> Loc { get; set; } = [];
        /// <summary>
        /// 
        /// </summary>
        public string? Msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Input { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string>? Ctx { get; set; }
    }
}
