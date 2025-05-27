namespace OpenElectricity.Sdk.Models
{
    public class Error422Response
    {
        public ErrorDetail? Detail { get; set; }
    }

    public class ErrorDetail
    {
        public List<string> Loc { get; set; } = [];
        public string? Msg { get; set; }
        public string? Type { get; set; }
        public string? Input { get; set; }
        public Dictionary<string, string>? Ctx { get; set; }
    }
}
