using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TeamyAPI.Models;

namespace TeamyAPI
{
    public class Startup
    {
        //SqlConnection con = new SqlConnection(@"Server=tcp:serveroicartim06.database.windows.net,1433;Initial Catalog=timeyDB;Persist Security Info=False;User ID=sas;Password=Pa$$w0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;");
        SqlConnection con = new SqlConnection(@"Data Source = (localdb)\mojLokalniDb;Initial Catalog=TeamyDB;Integrated Security=True");
        SqlCommand cmd;
        SqlDataReader dr;
        private List<Users> listUsers = new List<Users>();
        private List<Teams> listTeams = new List<Teams>();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConLoadUsers();
            ConLoadTeams();
        }

        public void ConLoadUsers()
        {
            cmd = new SqlCommand("getUsers", con);
            con.Open();
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listUsers.Add(new Users
                {
                    Id = (int)dr["IDUser"],
                    Name = dr["LoginName"].ToString(),
                    Roll = dr["Roll"].ToString(),
                    DateCreated = dr["DateCreated"].ToString()
                });
            }
            con.Close();
        }

        public void ConLoadTeams()
        {
            con.Open();
            cmd = new SqlCommand("getTeams", con);
            cmd.ExecuteNonQuery();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                listTeams.Add(new Teams
                {
                    Id = (int)dr["IDTeam"],
                    Name = dr["Team"].ToString(),
                    DateCreated = dr["Created"].ToString()
                });
            }
            con.Close();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<List<Users>>(listUsers);
            services.AddSingleton<List<Teams>>(listTeams);
            services.AddSingleton(con);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
