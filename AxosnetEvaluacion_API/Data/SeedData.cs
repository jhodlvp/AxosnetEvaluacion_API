using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxosnetEvaluacion_API.Data
{
    public static class SeedData
    {
        public async static Task Seed(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }
        private async static Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@axosEvaluacion.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin@axosEvaluacion.com",
                    Email = "admin@axosEvaluacion.com"
                };
                var result = await userManager.CreateAsync(user, "Admin.123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrador");
                }
            }

            if (await userManager.FindByEmailAsync("usuario@axosEvaluacion.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "usuario@axosEvaluacion.com",
                    Email = "usuario@axosEvaluacion.com"
                };
                var result = await userManager.CreateAsync(user, "User.123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "UsuarioLectura");
                }
            }
        }

        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Administrador"))
            {
                var role = new IdentityRole
                {
                    Name = "Administrador"
                };
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("UsuarioLectura"))
            {
                var role = new IdentityRole
                {
                    Name = "UsuarioLectura"
                };
                await roleManager.CreateAsync(role);
            }
        }
    }
}
