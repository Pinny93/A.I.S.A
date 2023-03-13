using A.I.S.A.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace A.I.S.A_.Digikabu
{
    public class DigikabuHandler
    {
        private const string DIGIKABU_KEY_FILE_PATH = @"Keys\digikabu.key";
        private static DigikabuUser? s_digikabuUser;
        private readonly RestClient _digikabuClient;
        private readonly string _digikabuApiKey;

        public DigikabuHandler()
        {
            _digikabuClient = new RestClient("https://digikabu.de/api/");
            _digikabuApiKey = File.ReadAllText(DIGIKABU_KEY_FILE_PATH);
        }

        public void Login(string user, TextWriter? outStreamWriter = null, string? password = null)
        {
            if (!AISA.IsInteractive && password == null)
            {
                outStreamWriter.WriteLine("This command is only supported in interactive mode!");
                return;
            }

            password ??= ConsoleUtil.ReadPassword() ?? "Anonymous";
            outStreamWriter.WriteLine();

            var request = new RestRequest($"authenticate?key={_digikabuApiKey}", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Authorization", $"Bearer {this.ApiKey}");
            request.AddJsonBody(JsonSerializer.Serialize(new DigikabuLoginSchema(user, password)));

            RestResponse? response = null;
            try
            {
                response = _digikabuClient.Execute(request);
            }
            catch (Exception ex)
            {
                outStreamWriter?.WriteLine(ex);
            }

            if (response is { IsSuccessful: false })
            {
                outStreamWriter?.WriteLine("Error occurred: " + response.StatusCode);
                return;
            }

            s_digikabuUser = JsonSerializer.Deserialize<DigikabuUser>(response?.Content ?? "");
            if (s_digikabuUser != null)
            {
                outStreamWriter?.WriteLine($"Sucessfully logged in as '{s_digikabuUser.Nachname}, {s_digikabuUser.Vorname}' ({s_digikabuUser.Klasse})!");
            }
        }

        public void Logout(TextWriter? outStreamWriter = null)
        {
            if (!EnsureLoggedIn(outStreamWriter)) { return; }

            s_digikabuUser = null;
            outStreamWriter?.WriteLine("Sucessfully logged out.");
        }

        internal IList<TResponse> GetDates<TResponse>(TextWriter? outStreamWriter = null, string url = "termine", int monat = -1, int idAbteilung = 1)
        {
            List<TResponse> list = new();
            if (!EnsureLoggedIn(outStreamWriter) || s_digikabuUser == null) { return list; }

            var request = new RestRequest(url, Method.Get);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {s_digikabuUser.Token}");

            // Add conditional parameters
            if (url == "termine/schulaufgaben")
            {
                request.AddParameter("monat", monat);
            }
            else if (url == "termine")
            {
                request.AddParameter("idAbteilung", idAbteilung);
            }

            RestResponse? response = null;
            response = _digikabuClient.Execute(request);

            if (response is { IsSuccessful: false })
            {
                outStreamWriter?.WriteLine("Error occurred: " + response.StatusCode);
                return list;
            }

            return JsonSerializer.Deserialize<List<TResponse>>(response.Content ?? "") ?? throw new ArgumentException("response"); ;
        }

        internal IList<DigikabuTimeTableLesson> GetLessons(TextWriter? outStreamWriter = null, DateTime date = default, int days = 1)
        {
            List<DigikabuTimeTableLesson> list = new();
            if (!EnsureLoggedIn(outStreamWriter) || s_digikabuUser == null) { return list; }

            if (date == default)
            {
                date = DateTime.Today;
            }

            var request = new RestRequest("stundenplan", Method.Get);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {s_digikabuUser.Token}");

            request.AddParameter("anzahl", days);
            request.AddParameter("datum", date.ToString("yyyy-MM-dd'T'HH:mm:ss"));

            RestResponse? response = null;
            response = _digikabuClient.Execute(request);

            if (response is { IsSuccessful: false })
            {
                outStreamWriter?.WriteLine("Error occurred: " + response.StatusCode);
                return list;
            }

            return JsonSerializer.Deserialize<List<DigikabuTimeTableLesson>>(response.Content ?? "") ?? throw new ArgumentException("response"); ;
        }

        internal bool EnsureLoggedIn(TextWriter? outStreamWriter)
        {
            if (this.IsLoggedIn)
            {
                outStreamWriter?.WriteLine("You are not logged in.");
                return false;
            }

            return true;
        }

        public bool IsLoggedIn => s_digikabuUser != null;
    }
}