using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teamy.Models;
using Teamy.Repository;

namespace Teamy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Login()
        {
            if ((string)Session["Uspjeh"] == "Uspjeh")
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(Users user)
        {
            int a = Repository.Repository.CheckLogin(user);
            if (a != -1)
            {
                Session["id"] = a.ToString();
                Session["Uspjeh"] = "Uspjeh";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.poruka = "Netočna kombinacija emaila i lozinke!";
                return View();
            }
        }

        public ActionResult Odjava()
        {
            Session["Uspjeh"] = "";

            return RedirectToAction("Login");
        }

        public ActionResult LeaveTeam()
        {
            Repository.Repository.LeaveTeam((string)Session["TeamName"], (string)Session["id"]);
            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UsersRegister user)
        {
            if (user.Pwd != user.PwdChecker)
            {
                return View();
            }
            else
            {
                Users user2 = new Users
                {
                    Pwd = user.Pwd,
                    Id = user.Id,
                    DateCreated = user.DateCreated,
                    Name = user.Name,
                    Roll = user.Roll
                };
                int a = Repository.Repository.CreateUser(user2);
                Session["id"] = a.ToString();
                Session["Uspjeh"] = "Uspjeh";
                return RedirectToAction("Index");
            }
        }

        public ActionResult EditProfile(string id)
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            Users a = Repository.Repository.GetUsers((string)Session["id"]);
            a.Pwd = "123456";
            return View(a);

        }

        [HttpPost]
        public ActionResult EditProfile(Users user)
        {
            user.Id = Int32.Parse((string)Session["id"]);
            Repository.Repository.ChangeUserName((string)Session["id"],user.Name);
            Users a = Repository.Repository.GetUsers((string)Session["id"]);
            a.Pwd = "123456";
            ViewBag.poruka = "Korisnik je ispravno unesen!";
            return View(a);

        }

        public ActionResult Index()
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            Users a = new Users();
            List<Teams> teams = new List<Teams>();
            teams = Repository.Repository.GetTeams((string)Session["id"]);
            if (teams.Count > 0)
            {
                foreach (var team in teams)
                {
                    if (team.TeacherID == -1)
                    {
                        team.TeacherName = "Profesor nije dodan";
                    }
                    else
                    {
                        a = Repository.Repository.GetUsers(team.TeacherID.ToString());
                        team.TeacherName = a.Name;
                    }
                    a = Repository.Repository.GetUsers(team.OwnerID.ToString());
                    team.OwnerName = a.Name;
                }
            }
            return View(teams);
        }

        public ActionResult JoinTeam()
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult JoinTeam(Teams team)
        {
            ViewBag.JoinTeam = "Request to join team has been sent!";
            Repository.Repository.JoinTeam((string)Session["id"], team.Name);
            return View();
        }

        public ActionResult CreateTeam()
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            Teams a = new Teams
            {
                OwnerName = Repository.Repository.GetUsers((string)Session["id"]).Name,
                DateCreated = DateTime.Now.ToShortDateString(),
                Name = ""
            };
            return View(a);
        }

        [HttpPost]
        public ActionResult CreateTeam(Teams team)
        {
            ViewBag.CreateTeam = "Team creation successfull!";
            team.OwnerID = Int32.Parse((string)Session["id"]);
            team.DateCreated = DateTime.Now.ToShortDateString();
            Repository.Repository.CreateTeam(team);
            team.OwnerName = Repository.Repository.GetUsers((string)Session["id"]).Name;
            return View(team);
        }

        public ActionResult Invites()
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            List<InviteUser> invites = new List<InviteUser>();
            invites = Repository.Repository.GetInvites((string)Session["id"]);
            return View(invites);
        }

        public ActionResult InvitesAccepted(string teamName)
        {
            Repository.Repository.JoinTeamThroughInvite(teamName, (string)Session["id"]);
            return RedirectToAction("Invites");
        }
 
        public ActionResult InvitesDismissed(string teamName, string userId)
        {
            Repository.Repository.DismissJoinTeamThroughInvite(teamName, userId);
            return RedirectToAction("Invites");
        }

        public ActionResult RequestAccepted(string id)
        {
            Repository.Repository.JoinUserToTeam((int)Session["TeamID"], id);
            return RedirectToAction("TeamJoinRequests");
        }

        public ActionResult RequestDismissed(string id)
        {
            Repository.Repository.DismissUserToTeam((int)Session["TeamID"], id);
            return RedirectToAction("TeamJoinRequests");
        }

        public ActionResult DismissWork(string workItemID)
        {
            Repository.Repository.DismissWork(workItemID);
            return RedirectToAction("TeamWorkCheck");
        }

        public ActionResult ReturnToWork(string id)
        {
            Repository.Repository.ReturnToWork(id);
            return RedirectToAction("TeamWorkCheckDeleted");
        }

        public ActionResult RemoveUser(string teamName, int userId)
        {
            Repository.Repository.RemoveUserFromTeam(teamName, userId);
            return RedirectToAction("Index");
        }

        public ActionResult MakeUserOwner(string teamName, int userId)
        {
            Repository.Repository.MakeUserOwner(teamName, userId);
            return RedirectToAction("Index");
        }

        public ActionResult MakeUserTeacher( int userId)
        {
            Repository.Repository.MakeUserTeacher(userId, (int)Session["TeamID"]);
            return RedirectToAction("Index");
        }

        public ActionResult TeamUsers(int id)
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            if (Repository.Repository.isUserInTeam((string)Session["id"], id) == true)
            {
                ViewBag.tim = Repository.Repository.GetTeam(id);
                Session["TeamName"] = ViewBag.tim;
                Session["TeamID"] = id;
                List<Users> teamUsers = new List<Users>();
                teamUsers = Repository.Repository.GetUsersForTeam(id);
                teamUsers.Add(Repository.Repository.GetTeacher(id));
                return View(teamUsers);
            }
            if (Repository.Repository.IsUserCreator((string)Session["id"],id) == true)
            {
                return RedirectToAction("TeamUsersAdmin",new {id = id});
            }
            if(Repository.Repository.IsUserTeacher((string)Session["id"],id) == true)
            {
                return RedirectToAction("TeamUsersTeacher", new {id = id});
            }
            return RedirectToAction("Index");
        }

        public ActionResult TeamUsersTeacher(int id)
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            ViewBag.tim = Repository.Repository.GetTeam(id);
            Session["TeamName"] = ViewBag.tim;
            Session["TeamID"] = id;
            List<TeamWork> work = new List<TeamWork>();
            work = Repository.Repository.GetWork((int)Session["TeamID"]);
            return View(work);
        }

        public ActionResult TeamUsersAdmin(int id)
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            if (Repository.Repository.IsUserCreator((string)Session["id"], id) == true)
            {
                ViewBag.tim = Repository.Repository.GetTeam(id);
                List<Users> teamUsers = new List<Users>();
                teamUsers = Repository.Repository.GetUsersForTeam(id);
                Session["TeamName"] = ViewBag.tim;
                Session["TeamID"] = id;
                teamUsers.Add(Repository.Repository.GetTeacher(id));
                return View(teamUsers);
            }
            return RedirectToAction("Index");
        }

        public ActionResult TeamWork()
        { 
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            else if(Repository.Repository.IsUserCreator((string)Session["id"], (int)Session["TeamID"]) == true)
            {
                return RedirectToAction("TeamWorkOwner");
            }
            else { return View(); }
        }

        public ActionResult TeamWorkOwner()
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            else { return View(); }
        }

        [HttpPost]
        public ActionResult TeamWorkOwner(TeamWork team)
        {
            TeamWork a = new TeamWork()
            {
                Name = team.Name,
                Description = team.Description,
                Date = DateTime.Now.ToShortDateString(),
                Start = DateTime.Now.ToString("HH:mm:ss"),
                TeamName = (string)Session["TeamName"],
                UserId = (string)Session["id"]

            };
            Session["Work"] = a;
            return RedirectToAction("StartTimer");
        }

        [HttpPost]
        public ActionResult TeamWork(TeamWork team)
        {
            TeamWork a = new TeamWork()
            {
                Name = team.Name,
                Description = team.Description,
                Date = DateTime.Now.ToShortDateString(),
                Start = DateTime.Now.ToString("HH:mm:ss"),
                TeamName = (string)Session["TeamName"],
                UserId = (string)Session["id"]

            };
            Session["Work"] = a;
            return RedirectToAction("StartTimer");
        }

        public ActionResult StartTimer()
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            else { return View(); }  
        }

        public ActionResult SaveWorkItem()
        {
            TeamWork a = (TeamWork)Session["Work"];
            a.End = DateTime.Now.ToString("HH:mm:ss");
            Repository.Repository.SaveWorkItem(a);
            return RedirectToAction("TeamUsers", new { id = (int)Session["TeamID"] });
        }

        public ActionResult TeamJoinRequests()
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            List<TeamRequest> requests = new List<TeamRequest>();
            requests = Repository.Repository.GetUsersWork((int)Session["TeamID"]);
            return View(requests);
        }

        public ActionResult TeamInvite()
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            else { return View(); }
        }

        [HttpPost]
        public ActionResult TeamInvite(string userName)
        {
            Repository.Repository.CreateInvite(userName,(int)Session["TeamID"]);
            ViewBag.Uspijeh = "User successfully invited!";
            return View();
        }

        public ActionResult TeamWorkCheck()
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            List<TeamWork> work = new List<TeamWork>();
            work = Repository.Repository.GetWork((int)Session["TeamID"]);
            return View(work);
        }

        public ActionResult TeamWorkCheckDeleted()
        {
            if ((string)Session["Uspjeh"] != "Uspjeh")
            {
                return RedirectToAction("Login");
            }
            List<TeamWork> work = new List<TeamWork>();
            work = Repository.Repository.GetWorkDeleted((int)Session["TeamID"]);
            return View(work);
        }

        public ActionResult DeleteTeam()
        {
            Repository.Repository.DeleteTeam((int)Session["TeamID"]);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteProfile()
        {
            Repository.Repository.DeleteProfile((string)Session["id"]);
            return RedirectToAction("Login");
        }

    }
}