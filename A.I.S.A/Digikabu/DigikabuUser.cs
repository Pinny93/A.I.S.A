using System.Text.Json.Serialization;

namespace A.I.S.A_.Digikabu;

public class DigikabuUser
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nachname")]
    public string? Nachname { get; set; }

    [JsonPropertyName("vorname")]
    public string? Vorname { get; set; }

    [JsonPropertyName("klasse")]
    public string? Klasse { get; set; }

    [JsonPropertyName("token")]
    public string? Token { get; set; }
}