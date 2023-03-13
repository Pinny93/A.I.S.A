using System.Text.Json.Serialization;
using A.I.S.A.Utils;

namespace A.I.S.A_.Digikabu;

public class DigikabuSA
{
    [JsonPropertyName("date")]
    [JsonConverter(typeof(MsCustomDateConverter))]
    public DateTime Date { get; set; }

    [JsonPropertyName("info")]
    public string Info { get; set; }
}