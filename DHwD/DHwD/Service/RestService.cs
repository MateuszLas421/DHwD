using DHwD.Interface;
using DHwD.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DHwD.Service
{
    class RestService: IRestService
    {
        HttpClient client;
        public RestService()
        {
            client = new HttpClient();
        }
        public async Task<bool> CheckUserExistsAsync(UserRegistration item)
        {
            bool isNewItem = false;
            Url_data url_ = null;
            HttpResponseMessage response = null;
            url_ = new Url_data();
            Uri uri;
            uri = new Uri(string.Format(url_.CheckLogin + item.NickName + "/" + item.Token));
            try { response = await client.GetAsync(uri.ToString()); }
            catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); return isNewItem; }
            if (response.IsSuccessStatusCode)  /// The user exists
            {
                Debug.WriteLine(@" The user exists. ");
                isNewItem=true;
                return isNewItem;
            }
            return isNewItem;
        }
        public async Task<bool> RegisterNewUserAsync(UserRegistration item)
        {

            Url_data url_=null;
            HttpResponseMessage response = null;
            string json = JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
             url_ = new Url_data(); 
            Uri uri;
            uri = new Uri(string.Format(url_.RegisterUri.ToString(), string.Empty));
            try { response = await client.PostAsync(uri, content); }                                      //  POST  // 
            catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); return false; }

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine(@"successfully saved.");
                return true;
            }
            return false;
        }
        public async Task<UserRegistration> GetUserAsync(UserRegistration item)
        {
            Url_data url_ = null;
            HttpResponseMessage response = null;
            url_ = new Url_data(); 
            Uri uri;
            uri = new Uri(string.Format(url_.CheckLogin + item.NickName + "/" + item.Token));
            try { response = await client.GetAsync(uri.ToString()); }               // REST GET
            catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); return null; }
            if (response.IsSuccessStatusCode)  /// when user exists
            {
                string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                var captureduser = JsonConvert.DeserializeObject<User>(responseContent);          // Deserialize JSON
                return captureduser;
            }
            return null;
        }

        public async Task<JWTToken> LoginAsync(UserRegistration item)
        {
            Url_data url_ = null;
            HttpResponseMessage response = null;
            url_ = new Url_data();
            Uri uri;
            uri = new Uri(string.Format(url_.CheckLogin + item.NickName + "/" + item.Token));
            try { response = await client.GetAsync(uri.ToString()); }               // REST GET
            catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); return null; }
            if (response.IsSuccessStatusCode)  /// when user exists
            {
                string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                var captured = JsonConvert.DeserializeObject<JWTToken>(responseContent);          // Deserialize JSON
                return captured;
            }
            return null;
        }

        public Task<Team> GetTeamAsync()  // NotImplemented
        {
            throw new NotImplementedException();
        }

        public async IAsyncEnumerable<Games> GetGames(JWTToken jWT)            // TODO
        {
            Url_data url_ = new Url_data();
            HttpResponseMessage response = null;
            List<Games> ListGames = null;
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            { 
                try { response = await client.GetAsync(url_.GameList.ToString()); }                // REST GET 
                catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); }
                if (response.IsSuccessStatusCode)  /// when user exists
                {
                   string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                   ListGames = JsonConvert.DeserializeObject<List<Games>>(responseContent);          // Deserialize JSON
                }
                else {/* return ;*/ } //???                                                     TODO
                for (int i = 0; i < ListGames.Count; i++)
                {
                    yield return ListGames[i];
                }
            }
        }
        public async IAsyncEnumerable<Team> GetTeams(JWTToken jWT, int IdGame)            // TODO
        {
            Url_data url_ = new Url_data();
            HttpResponseMessage response = null;
            List<Team> ListTeams = null;
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try { response = await client.GetAsync(url_.TeamList.ToString() + "all/" + IdGame); }                // REST GET 
                catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); }
                if (response.IsSuccessStatusCode)  /// when user exists
                {
                    string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                    ListTeams = JsonConvert.DeserializeObject<List<Team>>(responseContent);          // Deserialize JSON
                }
                else {/* return ;*/ } //???                                                     TODO
                for (int i = 0; i < ListTeams.Count; i++)
                {
                    yield return ListTeams[i];
                }
            }
        }
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
                catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); return false; }

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
                    catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); return false; }
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public async Task<bool> JoinToTeam(JWTToken jWT, Team item)
        {
            HttpResponseMessage response;
            string json = JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            Url_data url_ = new Url_data();
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try { response = await client.PostAsync(url_.TeamMembers.ToString(), content); }                                      //  POST  // 
                catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); return false; }
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<TeamMembers> GetMyTeams(JWTToken jWT, int idGame)
        {
            Url_data url_ = new Url_data();
            HttpResponseMessage response = null;
            TeamMembers ListTeams = null;
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try { response = await client.GetAsync(url_.TeamMembers_I.ToString() + "/" + idGame); }                // REST GET 
                catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); }
                if (response.IsSuccessStatusCode)  /// Status Code
                {
                    string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                    ListTeams = JsonConvert.DeserializeObject<TeamMembers>(responseContent);          // Deserialize JSON
                }
                else {/* return ;*/ } //???                                                     TODO 
                return ListTeams;
            }
        }
        public async IAsyncEnumerable<TeamMembers> GetTeamMembers(JWTToken jWT, int IdTeam)            // TODO
        {
            Url_data url_ = new Url_data();
            HttpResponseMessage response = null;
            List<TeamMembers> ListTeams = null;
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try { response = await client.GetAsync(url_.TeamMembers.ToString() + IdTeam); }                // REST GET 
                catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); }
                if (response.IsSuccessStatusCode)  /// when user exists
                {
                    string responseContent = await response.Content.ReadAsStringAsync();               // Read GET
                    ListTeams = JsonConvert.DeserializeObject<List<TeamMembers>>(responseContent);          // Deserialize JSON
                }
                else {/* return ;*/ } //???                                                     TODO
                for (int i = 0; i < ListTeams.Count; i++)
                {
                    yield return ListTeams[i];
                }
            }
        }

        public async Task<bool> CheckTeamPass(JWTToken jWT,int idteam, string hashpass)
        {
            Url_data url_ = new Url_data();
            HttpResponseMessage response = null;
            var authValue = new AuthenticationHeaderValue("Bearer", jWT.Token);
            using (var client = new HttpClient() { DefaultRequestHeaders = { Authorization = authValue } })
            {
                try { response = await client.GetAsync(url_.TeamList.ToString() + idteam + "/" + hashpass); }                // REST GET 
                catch (Exception ex) { Debug.WriteLine(ex.Message.ToString()); }
                if (response.IsSuccessStatusCode)  /// Status Code
                {
                    return true; // Password is correct
                }
                return false; // Password is incorrect
            }
        }
    }
}
