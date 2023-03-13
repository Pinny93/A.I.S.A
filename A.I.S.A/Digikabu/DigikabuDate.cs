using System.Text.Json.Serialization;
using A.I.S.A.Utils;

namespace A.I.S.A_.Digikabu;

public class DigikabuDate
{
    //"id": 2010,
    //"datumVon": "2022-09-28T09:00:00",
    //"datumBis": "2022-09-28T10:30:00",
    //"hinweis": "Aula wegen Veranstaltung gesperrt",
    //"art": 0,
    //"idabteilung": 1,
    //"klasse": null

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("datumVon")]
    [JsonConverter(typeof(MsCustomDateTimeConverter))]
    public DateTime DateFrom { get; set; }

    [JsonPropertyName("datumBis")]
    [JsonConverter(typeof(MsCustomDateTimeConverter))]
    public DateTime DateTo { get; set; }

    [JsonPropertyName("klasse")]
    public string? Klasse { get; set; }

    [JsonPropertyName("idabteilung")]
    public int IdAbteilung { get; set; }

    [JsonPropertyName("art")]
    public int Art { get; set; }

    [JsonPropertyName("hinweis")]
    public string Hinweis { get; set; }
}