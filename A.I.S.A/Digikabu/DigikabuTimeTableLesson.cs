using System.Text.Json.Serialization;
using A.I.S.A.Utils;

namespace A.I.S.A_.Digikabu;

public class DigikabuTimeTableLesson
{
    //"datum": "09.03.2023",
    //"anfStd": 1,
    //"endStd": 2,
    //"lehrer": "REII",
    //"uFachBez": "DB-I",
    //"raumLongtext": "D307",
    //"gruppe": "1/2"
    [JsonPropertyName("datum")]
    [JsonConverter(typeof(MsCustomDateConverter))]
    public DateTime Datum { get; set; }

    [JsonPropertyName("anfStd")]
    public int AnfStd { get; set; }

    [JsonPropertyName("endStd")]
    public int EndStd { get; set; }

    [JsonPropertyName("lehrer")]
    public string Lehrer { get; set; } = string.Empty;

    [JsonPropertyName("uFachBez")]
    public string UFachBez { get; set; } = string.Empty;

    [JsonPropertyName("raumLongtext")]
    public string Raum { get; set; } = string.Empty;

    [JsonPropertyName("gruppe")]
    public string Gruppe { get; set; } = string.Empty;
}