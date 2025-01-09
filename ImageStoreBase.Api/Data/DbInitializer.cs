using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Common.Define;
using Microsoft.AspNetCore.Identity;

namespace ImageStoreBase.Api.Data
{
    public class DbInitializer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public DbInitializer(AppDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedingData()
        {
            await SeedRoles();
            await SeedUsers();
            await SeedFunctionsAndCommands();
            await SeedCommandInFunctions();
            await SeedPermissions();
            
            await _context.SaveChangesAsync();
        }

        private async Task SeedRoles()
        {
            if (_roleManager.Roles.Any()) return;

            var roles = new[] 
            {
                MyApplicationDefine.Role.Admin,
                MyApplicationDefine.Role.User,
                MyApplicationDefine.Role.Admin
            };

            foreach (var roleName in roles)
            {
                await _roleManager.CreateAsync(new Role
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                });
            }
        }

        private async Task SeedUsers()
        {
            if (_userManager.Users.Any()) return;

            var users = new[]
            {
                new { Username = "admin", FirstName = "Quản trị", LastName = "1", 
                      Email = "thanhlanhlit@gmail.com", Password = "Admin@123456", Role = MyApplicationDefine.Role.Admin },
                new { Username = "user", FirstName = "Người dùng", LastName = "1", 
                      Email = "thanhlanh12a3@gmail.com", Password = "User@123456", Role = MyApplicationDefine.Role.User }
            };

            foreach (var userData in users)
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = userData.Username,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Email = userData.Email,
                    LockoutEnabled = false
                };

                var result = await _userManager.CreateAsync(user, userData.Password);
                if (result.Succeeded)
                {
                    var createdUser = await _userManager.FindByNameAsync(userData.Username);
                    await _userManager.AddToRoleAsync(createdUser, userData.Role);
                }
            }
        }

        private async Task SeedFunctionsAndCommands()
        {
            if (!_context.Functions.Any())
            {
                _context.Functions.AddRange(new List<Function>
                {
                    new Function {Id = nameof(MyApplicationDefine.Function.DASHBOARD), Name = MyApplicationDefine.Function.DASHBOARD, SortOrder = 1,Url = "/dashboard"  },
                    new Function {Id = nameof(MyApplicationDefine.Function.CONTENT), Name = MyApplicationDefine.Function.CONTENT, SortOrder = 2,Url = "/content"  },
                    new Function {Id = nameof(MyApplicationDefine.Function.STATISTIC), Name = MyApplicationDefine.Function.STATISTIC, SortOrder = 3,Url = "/statistic" },
                    new Function {Id = nameof(MyApplicationDefine.Function.SYSTEM), Name = MyApplicationDefine.Function.SYSTEM, SortOrder = 4, Url = "/system" },
                    new Function {Id = nameof(MyApplicationDefine.Function.SYSTEM_USER), Name = MyApplicationDefine.Function.SYSTEM_USER, SortOrder = 1,Url = "/system/user" },
                    new Function {Id = nameof(MyApplicationDefine.Function.SYSTEM_ROLE), Name = MyApplicationDefine.Function.SYSTEM_ROLE, SortOrder = 2,Url = "/system/role" },
                    new Function {Id = nameof(MyApplicationDefine.Function.SYSTEM_FUNCTION), Name = MyApplicationDefine.Function.SYSTEM_FUNCTION, SortOrder = 3,Url = "/system/function" },
                    new Function {Id = nameof(MyApplicationDefine.Function.SYSTEM_PERMISSION), Name = MyApplicationDefine.Function.SYSTEM_PERMISSION, SortOrder = 4,Url = "/system/permission" },
                });
                await _context.SaveChangesAsync();
            }

            if (!_context.Commands.Any())
            {
                _context.Commands.AddRange(new List<Command>()
                {
                    new Command(){Id = nameof(MyApplicationDefine.Command.VIEW), Name = MyApplicationDefine.Command.VIEW},
                    new Command(){Id = nameof(MyApplicationDefine.Command.CREATE), Name = MyApplicationDefine.Command.CREATE},
                    new Command(){Id = nameof(MyApplicationDefine.Command.UPDATE), Name = MyApplicationDefine.Command.UPDATE},
                    new Command(){Id = nameof(MyApplicationDefine.Command.DELETE), Name = MyApplicationDefine.Command.DELETE},
                    new Command(){Id = nameof(MyApplicationDefine.Command.APPROVE), Name = MyApplicationDefine.Command.APPROVE},
                });
            }
        }

        private async Task SeedCommandInFunctions()
        {
            var functions = _context.Functions;

            if (!_context.CommandInFunctions.Any())
            {
                foreach (var function in functions)
                {
                    var createAction = new CommandInFunction()
                    {
                        CommandId = nameof(MyApplicationDefine.Command.CREATE),
                        FunctionId = function.Id
                    };
                    _context.CommandInFunctions.Add(createAction);

                    var updateAction = new CommandInFunction()
                    {
                        CommandId = nameof(MyApplicationDefine.Command.UPDATE),
                        FunctionId = function.Id
                    };
                    _context.CommandInFunctions.Add(updateAction);
                    var deleteAction = new CommandInFunction()
                    {
                        CommandId = nameof(MyApplicationDefine.Command.DELETE),
                        FunctionId = function.Id
                    };
                    _context.CommandInFunctions.Add(deleteAction);

                    var viewAction = new CommandInFunction()
                    {
                        CommandId = nameof(MyApplicationDefine.Command.VIEW),
                        FunctionId = function.Id
                    };
                    _context.CommandInFunctions.Add(viewAction);
                }
            }
        }

        private async Task SeedPermissions()
        {
            if (!_context.Permissions.Any())
            {
                var adminRole = await _roleManager.FindByNameAsync(MyApplicationDefine.Role.Admin);
                var functions = _context.Functions;

                foreach (var function in functions)
                {
                    _context.Permissions.Add(new Permission { RoleId = adminRole.Id, FunctionId = function.Id, CommandId = nameof(MyApplicationDefine.Command.CREATE) });
                    _context.Permissions.Add(new Permission { RoleId = adminRole.Id, FunctionId = function.Id, CommandId = nameof(MyApplicationDefine.Command.UPDATE) });
                    _context.Permissions.Add(new Permission { RoleId = adminRole.Id, FunctionId = function.Id, CommandId = nameof(MyApplicationDefine.Command.DELETE) });
                    _context.Permissions.Add(new Permission { RoleId = adminRole.Id, FunctionId = function.Id, CommandId = nameof(MyApplicationDefine.Command.VIEW) });
                }
            }
        }
    }
}
