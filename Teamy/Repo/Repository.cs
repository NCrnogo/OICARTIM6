using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Teamy.Models;
using System.Text;


namespace Teamy.Repository
{
    public class Repository
    {
        private static readonly string url = "https://oicarapi.azurewebsites.net/api/";


        public static int CheckLogin(Users user)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var endpoint = new Uri(url + "User/uname=" + user.Name + "&pwd=" + user.Pwd);
                    var result = client.GetAsync(endpoint).Result;
                    var json = result.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<int>(json);
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        internal static int CreateUser(Users user)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "User");
                var newUserJson = JsonConvert.SerializeObject(user);
                var payload = new StringContent(newUserJson,Encoding.UTF8,"application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<int>(json);
            }
        }

        internal static void LeaveTeam(string teamName, string userId)
        {
            TeamAndUser teamAndUser = new TeamAndUser
            {
                IdUser = int.Parse(userId),
                TeamName = teamName
            };
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/LeaveTeam");
                var newUserJson = JsonConvert.SerializeObject(teamAndUser);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        public static Users GetUsers(string sessionId)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "User/id=" + sessionId);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Users>(json);
            }
        }

        internal static Users ChangeUserName(string id, string name)
        {
            Users user = new Users();
            user.Id = int.Parse(id);
            user.Name = name;
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "User/UpdateUser");
                var newUserJson = JsonConvert.SerializeObject(user);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Users>(json);
            }
        }

        public static Users GetUpdatedUser(Users user)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "User/UpdateUser");
                var newUserJson = JsonConvert.SerializeObject(user);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PutAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Users>(json);
            }
        }

        public static List<Teams> GetTeams(string sessionId)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/id=" + sessionId);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Teams>>(json);
            }
        }

        internal static void JoinTeam(string sessionId, string teamName)
        {
            InviteUser userInvite = new InviteUser
            {
                UserId = sessionId,
                TeamName = teamName
            };

            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/CreateInvite");
                var newUserJson = JsonConvert.SerializeObject(userInvite);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static void CreateTeam(Teams team)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/CreateTeam");
                var newUserJson = JsonConvert.SerializeObject(team);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static List<InviteUser> GetInvites(string sessionId)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/idInvitedUser=" + sessionId);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<InviteUser>>(json);
            }
        }

        internal static void JoinTeamThroughInvite(string teamName,string sessionId)
        {
            InviteUser userInvite = new InviteUser
            {
                UserId = sessionId,
                TeamName = teamName
            };
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/JoinTeamThroughInvite");
                var newUserJson = JsonConvert.SerializeObject(userInvite);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static void DismissJoinTeamThroughInvite(string teamName, string sessionId)
        {
            InviteUser userInvite = new InviteUser
            {
                UserId = sessionId,
                TeamName = teamName
            };
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/DismissJoinTeamThroughInvite");
                var newUserJson = JsonConvert.SerializeObject(userInvite);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static void ReturnToWork(string id)
        {
            TeamWork work = new TeamWork();
            work.idWork = id;
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/AcceptUsersWork");
                var newUserJson = JsonConvert.SerializeObject(work);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static void MakeUserTeacher(int userId, int teamId)
        {
            TeamRequest userTeacher = new TeamRequest();
            userTeacher.UserId = userId.ToString();
            userTeacher.TeamId = teamId.ToString();
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/MakeUserTeacher");
                var newUserJson = JsonConvert.SerializeObject(userTeacher);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static Users GetTeacher(int id)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "User/isTeacher=" + id);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                var a = JsonConvert.DeserializeObject<Users>(json);
                return a;
            }
        }

        internal static void DismissWork(string id)
        {
            TeamWork work = new TeamWork();
            work.idWork = id;
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/DismissUsersWork");
                var newUserJson = JsonConvert.SerializeObject(work);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static void DismissUserToTeam(int idTeam, string userId)
        {
            TeamRequest teamRequest = new TeamRequest
            {
                UserId=userId,
                TeamId=idTeam.ToString(),
                UserName = ""
            };
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/DismissUserToTeam");
                var newUserJson = JsonConvert.SerializeObject(teamRequest);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static void JoinUserToTeam(int idTeam, string userId)
        {
            TeamRequest teamRequest = new TeamRequest
            {
                UserId = userId,
                TeamId = idTeam.ToString(),
                UserName = ""
            };
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/JoinUserToTeam");
                var newUserJson = JsonConvert.SerializeObject(teamRequest);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static void MakeUserOwner(string teamName, int userId)
        {
            TeamAndUser teamAndUser = new TeamAndUser
            {
                IdUser = userId,
                TeamName = teamName
            };
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/MakeUserOwner");
                var newUserJson = JsonConvert.SerializeObject(teamAndUser);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static void RemoveUserFromTeam(string teamName, int userId)
        {
            TeamAndUser teamAndUser = new TeamAndUser
            {
                IdUser = userId,
                TeamName = teamName
            };
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/RemoveUserFromTeam");
                var newUserJson = JsonConvert.SerializeObject(teamAndUser);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static void CreateInvite(string userName, int idTeam)
        {
            InviteTeam teamInvite = new InviteTeam
            {
                UserName = userName,
                TeamId = idTeam.ToString()
            };

            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/CreateInviteUser");
                var newUserJson = JsonConvert.SerializeObject(teamInvite);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static string GetTeam(int id)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/idTeam=" + id);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                var a = JsonConvert.DeserializeObject<Teams>(json);
                return a.Name;
            }
        }

        internal static bool IsUserTeacher(string idUser, int idTeam)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "User/isTeacher=" + idTeam);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                var a = JsonConvert.DeserializeObject<Users>(json);
                return a.Id == Int32.Parse(idUser);
            }
        }

        internal static void DeleteProfile(string id)
        {
            InviteUser a = new InviteUser();
            a.UserId = id;
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/DeleteUserProfileWithID");
                var newUserJson = JsonConvert.SerializeObject(a);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static void SaveWorkItem(TeamWork a)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/SaveWorkItem");
                var newUserJson = JsonConvert.SerializeObject(a);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static void DeleteTeam(int teamID)
        {
            InviteTeam a = new InviteTeam();
            a.TeamId = teamID.ToString();
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/DeleteTeamWithID");
                var newUserJson = JsonConvert.SerializeObject(a);
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payload).Result;
                var json = result.Content.ReadAsStringAsync().Result;
            }
        }

        internal static bool isUserInTeam(string idUser, int idTeam)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "User/" + idUser + "/" + idTeam);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                var a = JsonConvert.DeserializeObject<Users>(json);
                return a.Id == Int32.Parse(idUser);
            }
        }

        internal static List<TeamWork> GetWork(int teamId)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/TeamIdForWork=" + teamId);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<TeamWork>>(json);
            }
        }

        internal static List<TeamWork> GetWorkDeleted(int teamId)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/TeamIdForWorkDeleted=" + teamId);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<TeamWork>>(json);
            }
        }

        internal static List<TeamRequest> GetUsersWork(int teamID)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "Teams/idOfTeam=" + teamID);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<TeamRequest>>(json);
            }
        }

        internal static bool IsUserCreator(string idUser, int idTeam)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "User/isOwner=" + idTeam);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                var a = JsonConvert.DeserializeObject<Users>(json);
                return a.Id == Int32.Parse(idUser);
            }
        }
        
        internal static List<Users> GetUsersForTeam(int id)
        {
            using (var client = new HttpClient())
            {
                var endpoint = new Uri(url + "User/team=" + id);
                var result = client.GetAsync(endpoint).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Users>>(json);
            }
        }
    }
}