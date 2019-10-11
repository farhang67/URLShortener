using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Core.Domain;

namespace Infrastructure
{
    public static class ApplicationDbContextSeedData
    {
        public static async void SeedData(this IServiceScopeFactory serviceScopeFactory, IConfiguration configuraion)
        {



            //using (var serviceScope = scopeFactory.CreateScope())
            //{
            //    var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            //    if (!context.Categories.Any())
            //    {
            //        var categories = new List<Category>
            //        {
            //            new Category
            //            {
            //              Tilte = "دسته بندی تست"

            //            }
            //        };
            //        context.AddRange(categories);
            //        context.SaveChanges();
            //    }
            //}




            using (var serviceScope = serviceScopeFactory.CreateScope())
            {

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                //initializing custom roles 
                //var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                //var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string[] roleNames = { "Admin", "Member" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        //create the roles and seed them to the database: Question 1
                        roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }


                //Here you could create a super user who will maintain the web app
                var poweruser = new ApplicationUser
                {
                    UserName = configuraion["SuperUser:Email"],
                    Email = configuraion["SuperUser:Email"],
                };
                //Ensure you have these values in your appsettings.json file
                string userPWD = configuraion["SuperUser:Password"];
                var _user = await userManager.FindByEmailAsync(configuraion["SuperUser:Email"]);

                if (_user == null)
                {
                    //create user
                    var createPowerUser = await userManager.CreateAsync(poweruser, userPWD);

                    //add user to role
                    if (createPowerUser.Succeeded)
                    {
                        //here we tie the new user to the role
                        await userManager.AddToRoleAsync(poweruser, "Admin");

                    }                 

                }
            }
        }
    }

}
