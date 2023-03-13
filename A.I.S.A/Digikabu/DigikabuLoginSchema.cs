using System.ComponentModel.DataAnnotations;

namespace A.I.S.A_.Digikabu;

public class DigikabuLoginSchema
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public DigikabuLoginSchema(string user, string password)
    {
        Username = user;
        Password = password;
    }
    public DigikabuLoginSchema() { }
}