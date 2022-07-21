using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using TeamyAPI.Models;

namespace TeamyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {

        SqlConnection Con;
        SqlDataReader dr;
        SqlCommand cmd;
        private List<Teams> ListTeams;

        public TeamsController(List<Teams> listTeams, SqlConnection con)
        {
            ListTeams = listTeams;
            Con = con;
        }

        //call http://localhost:5000/api/Teams
        [HttpGet]
        public List<Teams> Get()
        {
            return ListTeams;
        }

        //call http://localhost:5000/api/Teams/idTeam=1
        //Dohvacanje odredjenog tima po idu tima
        [HttpGet("idTeam={id}")]
        public Teams GetTeam(int id)
        {
            Teams a = new Teams();
            cmd = new SqlCommand("getTeam", Con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
            Con.Open();
            cmd.Connection = Con;
            cmd.ExecuteNonQuery();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                a= new Teams
                {
                    Id = (int)dr["IDTeam"],
                    Name = dr["Team"].ToString(),
                    DateCreated = dr["Created"].ToString()
                };
            }
            Con.Close();
            return a;
        }

        //call http://localhost:5000/api/Teams/id=1
        //Dohvacanje odredjenog tima po idu usera
        [HttpGet("id={id}")]
        public List<Teams> GetTeams(int id)
        {
            List<Teams> a = new List<Teams>();
            cmd = new SqlCommand("getTeamsByUser", Con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
            Con.Open();
            cmd.Connection = Con;
            cmd.ExecuteNonQuery();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                a.Add(new Teams
                {
                    Id = (int)dr["IDTeam"],
                    Name = dr["Team"].ToString(),
                    DateCreated = dr["Created"].ToString(),
                    TeacherID = dr["TeacherID"] == null ? (int)dr["TeacherID"] : -1,
                    OwnerID = (int)dr["OwnerID"]
                });
            }
            Con.Close();
            return a;
        }
        //Dohvaca sve zahtjeve za uclanjenje u tim po useru
        [HttpGet("idInvitedUser={id}")]
        public List<InviteUser> GetInvites(int id)
        {
            List<InviteUser> a = new List<InviteUser>();
            cmd = new SqlCommand("getTeamInvites", Con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = id;
            Con.Open();
            cmd.Connection = Con;
            cmd.ExecuteNonQuery();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                a.Add(new InviteUser
                {
                    UserId = dr["UserID"].ToString(),
                    TeamName = dr["Team"].ToString()
                });
            }
            Con.Close();
            return a;
        }
        //Dohvaca sve zahtjeve za uclanjenje u tim po timu
        [HttpGet("idOfTeam={id}")]
        public List<TeamRequest> GetRequests(string id)
        {
            List<TeamRequest> a = new List<TeamRequest>();
            cmd = new SqlCommand("getJoinRequests", Con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@idTeam", System.Data.SqlDbType.Int).Value = int.Parse(id);
            Con.Open();
            cmd.Connection = Con;
            cmd.ExecuteNonQuery();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                a.Add(new TeamRequest
                {
                    UserId = dr["UserID"].ToString(),
                    UserName = dr["LoginName"].ToString(),
                    TeamId = dr["TeamID"].ToString()
                });
            }
            Con.Close();
            return a;
        }
        //Dohacanje svih radova clanova tima po idu tima
        [HttpGet("TeamIdForWork={id}")]
        public List<TeamWork> GetWork(int id)
        {
            List<TeamWork> a = new List<TeamWork>();
            cmd = new SqlCommand("GetWork", Con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@idTeam", System.Data.SqlDbType.Int).Value = id;
            Con.Open();
            cmd.Connection = Con;
            cmd.ExecuteNonQuery();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                a.Add(new TeamWork
                {
                    Date = dr["DateCreated"].ToString(),
                    Name = dr["Name"].ToString(),
                    Description = dr["Details"].ToString(),
                    TeamName = "",
                    End = dr["EndWork"].ToString(),
                    Start = dr["StartWork"].ToString(),
                    idWork = dr["IDUserWork"].ToString(),
                    UserName = dr["LoginName"].ToString()
                });
            }
            Con.Close();
            return a;
        }
        //Micanje odredenog rada korisnika po idu
        [HttpGet("TeamIdForWorkDeleted={id}")]
        public List<TeamWork> GetWorkDeleted(int id)
        {
            List<TeamWork> a = new List<TeamWork>();
            cmd = new SqlCommand("GetWorkDeleted", Con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("@idTeam", System.Data.SqlDbType.Int).Value = id;
            Con.Open();
            cmd.Connection = Con;
            cmd.ExecuteNonQuery();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                a.Add(new TeamWork
                {
                    Date = dr["DateCreated"].ToString(),
                    Name = dr["Name"].ToString(),
                    Description = dr["Details"].ToString(),
                    TeamName = "",
                    End = dr["EndWork"].ToString(),
                    Start = dr["StartWork"].ToString(),
                    idWork = dr["IDUserWork"].ToString(),
                    UserName = dr["LoginName"].ToString()
                });
            }
            Con.Close();

            return a;
        }
        //Kreira invajt od usera prema timu
        [HttpPost]
        [Route("CreateInvite")]
        public void CreateInvite(InviteUser userInvite)
        {
            try
            {
                cmd = new SqlCommand("joinRequestUser", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@teamName", System.Data.SqlDbType.VarChar).Value = userInvite.TeamName;
                cmd.Parameters.Add("@userId", System.Data.SqlDbType.Int).Value = int.Parse(userInvite.UserId);
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        //Ubacivanje usera u tim nakon uspjenog prihvacanja
        [HttpPost]
        [Route("JoinTeamThroughInvite")]
        public void JoinTeamThroughInvite(InviteUser userInvite)
        {
            try
            {
                cmd = new SqlCommand("joinTeam", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = int.Parse(userInvite.UserId);
                cmd.Parameters.Add("@teamName", System.Data.SqlDbType.VarChar).Value = userInvite.TeamName;
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        //Odbijanja nekog invajta i micanje iz baze (user -> team)
        [HttpPost]
        [Route("DismissJoinTeamThroughInvite")]
        public void DismissJoinTeamThroughInvite(InviteUser userInvite)
        {
            try
            {
                cmd = new SqlCommand("dismissJoinTeam", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = int.Parse(userInvite.UserId);
                cmd.Parameters.Add("@teamName", System.Data.SqlDbType.VarChar).Value = userInvite.TeamName;
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        //Odbijanje nekog innvajta i micanje iz baze (team->user)
        [HttpPost]
        [Route("DismissUserToTeam")]
        public void DismissUserToTeam(TeamRequest teamRequest)
        {
            try
            {
                cmd = new SqlCommand("DismissUserToTeam", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = int.Parse(teamRequest.UserId);
                cmd.Parameters.Add("@idTeam", System.Data.SqlDbType.Int).Value = int.Parse(teamRequest.TeamId);
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        //Prihvacanje invajta od nekog tima
        [HttpPost]
        [Route("JoinUserToTeam")]
        public void JoinUserToTeam(TeamRequest teamRequest)
        {
            try
            {
                cmd = new SqlCommand("JoinUserToTeam", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = int.Parse(teamRequest.UserId);
                cmd.Parameters.Add("@idTeam", System.Data.SqlDbType.Int).Value = int.Parse(teamRequest.TeamId);
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        //Kreiranje zahtjeva za pridruženje timu
        [HttpPost]
        [Route("CreateInviteUser")]
        public void CreateInvite(InviteTeam invite)
        {
            try
            {
                cmd = new SqlCommand("joinRequestTeam", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@userName", System.Data.SqlDbType.VarChar).Value = invite.UserName;
                cmd.Parameters.Add("@teamid", System.Data.SqlDbType.Int).Value = int.Parse(invite.TeamId);
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        //Nakon uspješnog klika gumba micanje određenog subjekta iz tima
        [HttpPost]
        [Route("RemoveUserFromTeam")]
        public void RemoveUserFromTeam(TeamAndUser teamAndUser)
        {
            try
            {
                cmd = new SqlCommand("removeFromTeam", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = teamAndUser.IdUser;
                cmd.Parameters.Add("@teamName", System.Data.SqlDbType.VarChar).Value = teamAndUser.TeamName;
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        // Nakon uspješnog klika na gumb mjenjanje glavnog korisnika u obicnog usera i određenog usera na glavnog korisnika
        [HttpPost]
        [Route("MakeUserOwner")]
        public void MakeUserOwner(TeamAndUser teamAndUser)
        {
            try
            {
                cmd = new SqlCommand("makeOwnerOfTeam", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = teamAndUser.IdUser;
                cmd.Parameters.Add("@teamName", System.Data.SqlDbType.VarChar).Value = teamAndUser.TeamName;
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        // Kreiranje tima
        [HttpPost]
        [Route("CreateTeam")]
        public void CreateTeam(Teams team)
        {
            try
            {
                cmd = new SqlCommand("createTeam", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = team.OwnerID;
                cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = team.Name;
                cmd.Parameters.Add("@created", System.Data.SqlDbType.VarChar).Value = team.DateCreated;
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        // Odbijanje rada nekog korisnika
        [HttpPost]
        [Route("DismissUsersWork")]
        public void DismissUsersWork(TeamWork work)
        {
            try
            {
                cmd = new SqlCommand("DismissWork", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idWork", System.Data.SqlDbType.Int).Value = int.Parse(work.idWork);
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        //Prihvacanje rada nakon njegovog odbijanja
        [HttpPost]
        [Route("AcceptUsersWork")]
        public void AcceptUsersWork(TeamWork work)
        {
            try
            {
                cmd = new SqlCommand("AcceptUsersWork", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idWork", System.Data.SqlDbType.Int).Value = int.Parse(work.idWork);
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        //Izalazak iz tima kao korisnik tima
        [HttpPost]
        [Route("LeaveTeam")]
        public void LeaveTeam(TeamAndUser teamAndUser)
        {
            try
            {
                cmd = new SqlCommand("removeFromTeam", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = teamAndUser.IdUser;
                cmd.Parameters.Add("@teamName", System.Data.SqlDbType.VarChar).Value = teamAndUser.TeamName;
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        //Uklanjanje tima kada smo owner tima
        [HttpPost]
        [Route("DeleteTeamWithID")]
        public void DeleteTeamWithID(InviteTeam teamId)
        {
            try
            {
                cmd = new SqlCommand("DeleteTeam", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idTeam", System.Data.SqlDbType.Int).Value = int.Parse(teamId.TeamId);
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        //Spremanje rada korisnika u timu
        [HttpPost]
        [Route("SaveWorkItem")]
        public void SaveWorkItem(TeamWork team)
        {
            try
            {
                cmd = new SqlCommand("saveWork", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = team.UserId;
                cmd.Parameters.Add("@name", System.Data.SqlDbType.VarChar).Value = team.Name;
                cmd.Parameters.Add("@teamName", System.Data.SqlDbType.VarChar).Value = team.TeamName;
                cmd.Parameters.Add("@description", System.Data.SqlDbType.VarChar).Value = team.Description;
                cmd.Parameters.Add("@start", System.Data.SqlDbType.VarChar).Value = team.Start;
                cmd.Parameters.Add("@end", System.Data.SqlDbType.VarChar).Value = team.End;
                cmd.Parameters.Add("@date", System.Data.SqlDbType.VarChar).Value = team.Date;
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
        /// <summary>
        /// OD TU
        /// </summary>
        /// <param name="team"></param>
        [HttpPost]
        [Route("MakeUserTeacher")]
        public void MakeUserTeacher(TeamRequest team)
        {
            try
            {
                cmd = new SqlCommand("MakeUserTeacher", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = int.Parse(team.UserId);
                cmd.Parameters.Add("@idTeam", System.Data.SqlDbType.Int).Value = int.Parse(team.TeamId);
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }

        [HttpPost]
        [Route("DeleteUserProfileWithID")]
        public void DeleteUserProfileWithID(InviteUser team)
        {
            try
            {
                cmd = new SqlCommand("MakeUserTeacher", Con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@idUser", System.Data.SqlDbType.Int).Value = int.Parse(team.UserId);
                Con.Open();
                cmd.Connection = Con;
                cmd.ExecuteNonQuery();
                Con.Close();
                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            }
            catch
            {
                Con.Close();
            }
        }
    }
}
