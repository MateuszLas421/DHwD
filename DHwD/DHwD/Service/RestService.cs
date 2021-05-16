using DHwD.Interface;
using DHwD.Models;
using Microsoft.AppCenter.Crashes;
using Models.ModelsDB;
using Models.ModelsMobile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DHwD.Service
{
    class RestService : IRestService   // TODO Generic !!!
    {
        HttpClient client;
        public RestService()
        {
            client = new HttpClient();
        }

        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public HttpClient Prepare(JWTToken jWT)
        {
            Url_data url_ = new Url_data();
            HttpResponseMessage response = null;
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } };
            return client;
        }

        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public async Task<bool> CheckUserExistsAsync(UserRegistration item)
        {
            bool isNewItem = false;
            Url_data url_ = null;
            HttpResponseMessage response = null;
            url_ = new Url_data();
            Uri uri;
            uri = new Uri(string.Format(url_.CheckLogin + item.NickName + "/" + item.Token));
            try { response = await client.GetAsync(uri.ToString()); }
            catch (Exception ex) 
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message.ToString());
                return isNewItem; 
            }
            if (response.IsSuccessStatusCode)  /// The user exists
            {
                Debug.WriteLine(@" The user exists. ");
                isNewItem = true;
                return isNewItem;
            }
            return isNewItem;
        }

        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public async Task<bool> RegisterNewUserAsync(UserRegistration item)
        {
            Url_data url_ = null;
            HttpResponseMessage response = null;
            string json = JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            url_ = new Url_data();
            Uri uri;
            uri = new Uri(string.Format(url_.RegisterUri.ToString(), string.Empty));
            try { response = await client.PostAsync(uri, content); }                                      //  POST  // 
            catch (Exception ex) 
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message.ToString()); 
                return false; 
            }

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"successfully saved.");
                return true;
            }
            return false;
        }

        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public async Task<UserRegistration> GetUserAsync(UserRegistration item)
        {
            Url_data url_ = null;
            HttpResponseMessage response = null;
            url_ = new Url_data();
            Uri uri;
            uri = new Uri(string.Format(url_.CheckLogin + item.NickName + "/" + item.Token));
            try { response = await client.GetAsync(uri.ToString()); }               // REST GET
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message.ToString()); 
                return null; 
            }
            if (response.IsSuccessStatusCode)  /// when user exists
            {
                string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                var captureduser = JsonConvert.DeserializeObject<UserRegistration>(responseContent);          // Deserialize JSON
                return captureduser;
            }
            return null;
        }

        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public async Task<JWTToken> LoginAsync(UserRegistration item)
        {
            Url_data url_ = null;
            HttpResponseMessage response = null;
            url_ = new Url_data();
            Uri uri;
            uri = new Uri(string.Format(url_.CheckLogin + item.NickName + "/" + item.Token));
            try { response = await client.GetAsync(uri.ToString()); }               // REST GET
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message.ToString()); 
                return null; 
            }
            if (response.IsSuccessStatusCode)  /// when user exists
            {
                string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                var captured = JsonConvert.DeserializeObject<JWTToken>(responseContent);          // Deserialize JSON
                return captured;
            }
            return null;
        }

        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", true)]
        public Task<Team> GetTeamAsync()  // NotImplemented
        {
            throw new NotImplementedException();
        }

        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public async IAsyncEnumerable<Games> GetGames(JWTToken jWT)
        {
            Url_data url_ = new Url_data();
            HttpResponseMessage response = null;
            List<Games> ListGames = null;
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try { response = await client.GetAsync(url_.GameList.ToString()); }                // REST GET 
                catch (Exception ex) 
                {
                    Crashes.TrackError(ex);
                    Debug.WriteLine(ex.Message.ToString());
                }
                if (response.IsSuccessStatusCode)  /// when user exists
                {
                    string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                    ListGames = JsonConvert.DeserializeObject<List<Games>>(responseContent);          // Deserialize JSON
                }
                else { yield return null; }
                for (int i = 0; i < ListGames.Count; i++)
                {
                    yield return ListGames[i];
                }
            }
        }
        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public async IAsyncEnumerable<MobileTeam> GetTeams(JWTToken jWT, int IdGame)
        {
            Url_data url_ = new Url_data();
            HttpResponseMessage response = null;
            List<MobileTeam> ListTeams = null;
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try
                {
                    response = await client.GetAsync(url_.TeamList.ToString() + "all/" + IdGame);    // REST GET 
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    Debug.WriteLine(ex.Message.ToString());
                }
                if (response.IsSuccessStatusCode)  /// when user exists
                {
                    string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                    ListTeams = JsonConvert.DeserializeObject<List<MobileTeam>>(responseContent);          // Deserialize JSON
                }
                else { yield return null; }
                for (int i = 0; i < ListTeams.Count; i++)
                {
                    yield return ListTeams[i];
                }
            }
        }

        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public async Task<bool> CreateNewTeam(JWTToken jWT, Team item)
        {
            HttpResponseMessage response;
            string json = JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            Url_data url_ = new Url_data();
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try { response = await client.PostAsync(url_.TeamList.ToString(), content); }                                      //  POST  // 
                catch (Exception ex) 
                {
                    Crashes.TrackError(ex);
                    Debug.WriteLine(ex.Message.ToString()); 
                    return false; 
                }

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    TeamMembers teamMembers = new TeamMembers();
                    var deserializeJSON= JsonConvert.DeserializeObject<Team>(responseContent);
                    teamMembers.Team = item;
                    teamMembers.Team.Id = deserializeJSON.Id;
                    json = JsonConvert.SerializeObject(teamMembers);
                    content = new StringContent(json, Encoding.UTF8, "application/json");
                    try { response = await client.PostAsync(url_.TeamMembersNewTeam.ToString(), content); }                                      //  POST  // 
                    catch (Exception ex) 
                    {
                        Crashes.TrackError(ex);
                        Debug.WriteLine(ex.Message.ToString()); return false; 
                    }
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public async Task<bool> JoinToTeam(JWTToken jWT, Team item)
        {
            HttpResponseMessage response;
            string json = JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            Url_data url_ = new Url_data();
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try
                {
                    response = await client.PostAsync(url_.TeamMembers.ToString(), content); 
                }                                      //  POST  // 
                catch (Exception ex) 
                {
                    Crashes.TrackError(ex);
                    Debug.WriteLine(ex.Message.ToString()); 
                    return false; 
                }
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public async Task<TeamMembers> GetMyTeams(JWTToken jWT, int idGame)
        {
            Url_data url_ = new Url_data();
            HttpResponseMessage response = null;
            TeamMembers ListTeams = null;
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try { response = await client.GetAsync(url_.TeamMembers_I.ToString() + "/" + idGame); }                // REST GET 
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    Debug.WriteLine(ex.Message.ToString()); 
                }
                if (response.IsSuccessStatusCode)  /// Status Code
                {
                    string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                    ListTeams = JsonConvert.DeserializeObject<TeamMembers>(responseContent);          // Deserialize JSON
                }
                else {  return null; } 
                return ListTeams;
            }
        }

        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public async IAsyncEnumerable<TeamMembers> GetTeamMembers(JWTToken jWT, int IdTeam) 
        {
            Url_data url_ = new Url_data();
            HttpResponseMessage response = null;
            List<TeamMembers> ListTeams = null;
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try { response = await client.GetAsync(url_.TeamMembers.ToString() + IdTeam); }                // REST GET 
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    Debug.WriteLine(ex.Message.ToString()); 
                }
                if (response.IsSuccessStatusCode)  /// when user exists
                {
                    string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                    ListTeams = JsonConvert.DeserializeObject<List<TeamMembers>>(responseContent);          // Deserialize JSON
                }
                else { yield return null; } 
                for (int i = 0; i < ListTeams.Count; i++)
                {
                    yield return ListTeams[i];
                }
            }
        }

        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public async Task<bool> CheckTeamPass(JWTToken jWT,int idteam, string hashpass)
        {
            Url_data url_ = new Url_data();
            HttpResponseMessage response = null;
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try { response = await client.GetAsync(url_.TeamList.ToString() + idteam + "/" + hashpass); }                // REST GET 
                catch (Exception ex) 
                {
                    Crashes.TrackError(ex);
                    Debug.WriteLine(ex.Message.ToString());
                }
                if (response.IsSuccessStatusCode)  /// Status Code
                {
                    return true; // Password is correct
                }
                return false; // Password is incorrect
            }
        }

        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public async Task<List<Location>> GetLocationAsync(JWTToken jWT,Team team)
        {
            Url_data url_ = new Url_data();
            HttpResponseMessage response = null;
            List<Location> location = new List<Location>();
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try { response = await client.GetAsync(url_.Location.ToString() + team.Id); }                // REST GET 
                catch (Exception ex) 
                {
                    Crashes.TrackError(ex);
                    Debug.WriteLine(ex.Message.ToString()); 
                }
                if (response.IsSuccessStatusCode)  /// Status Code
                {
                    string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                    location = JsonConvert.DeserializeObject<List<Location>>(responseContent);          // Deserialize JSON
                }
                else { return null;   } 
                return location;
            }
        }
        [ObsoleteAttribute("This method is obsolete. Use Generic BaseREST instead.", false)]
        public async IAsyncEnumerable<Chats> GetChat(JWTToken jWT, int IdGame)
        {
            Url_data url_ = new Url_data();
            HttpResponseMessage response = null;
            List<Chats> List = null;
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try
                {
                    response = await client.GetAsync(url_.Chat.ToString() + "Game=" + IdGame);    // REST GET 
                    if (response.IsSuccessStatusCode)  /// when user exists
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                        List = JsonConvert.DeserializeObject<List<Chats>>(responseContent);          // Deserialize JSON
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    Debug.WriteLine(ex.Message.ToString());
                }
                if (response.StatusCode==System.Net.HttpStatusCode.NotFound)
                {
                    List = new List<Chats>();
                }
                else
                {
                    yield return new Chats();
                }
                for (int i = List.Count-1; i >=0; i--)
                {
                    yield return List[i];
                }
                if (List.Count == 0)
                {
                    yield return new Chats();
                }
            }
        }
    }
}
