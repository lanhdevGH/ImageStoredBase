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
            #region RoleDataInit
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new Role
                {
                    Name = MyApplicationDefine.Customer,
                    NormalizedName = MyApplicationDefine.Customer.ToUpper(),
                });
                await _roleManager.CreateAsync(new Role
                {
                    Name = MyApplicationDefine.User,
                    NormalizedName = MyApplicationDefine.User.ToUpper(),
                });
                await _roleManager.CreateAsync(new Role
                {
                    Name = MyApplicationDefine.Admin,
                    NormalizedName = MyApplicationDefine.Admin.ToUpper(),
                });
            }
            #endregion

            #region UserDataInit
            if (!_userManager.Users.Any())
            {
                var result = await _userManager.CreateAsync(new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "admin",
                    FirstName = "Quản trị",
                    LastName = "1",
                    Email = "thanhlanhlit@gmail.com",
                    LockoutEnabled = false
                }, "Admin@123");
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("admin");
                    await _userManager.AddToRoleAsync(user, MyApplicationDefine.Admin);
                }

                var result2 = await _userManager.CreateAsync(new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "user",
                    FirstName = "Người dùng",
                    LastName = "1",
                    Email = "thanhlanhlit@gmail.com",
                    LockoutEnabled = false
                }, "user@123");
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("user");
                    await _userManager.AddToRoleAsync(user, MyApplicationDefine.User);
                }
            }
            #endregion

            #region Functon&CommandDataInit

            if (!_context.Functions.Any())
            {
                _context.Functions.AddRange(new List<Function>
                {
                    new Function {Id = "DASHBOARD", Description="Trang chủ", ParentId = null, SortOrder = 1,Url = "/dashboard"  },

                    new Function {Id = "CONTENT",Description="Nội dung",ParentId = null,Url = "/content" },
                    new Function {Id = "STATISTIC",Description="Thống kê", ParentId = null, Url = "/statistic" },
                    new Function {Id = "SYSTEM", Description="Hệ thống", ParentId = null, Url = "/system" },

                    new Function {Id = "SYSTEM_USER", Description="Người dùng",ParentId = "SYSTEM",Url = "/system/user"},
                    new Function {Id = "SYSTEM_ROLE", Description="Nhóm quyền",ParentId = "SYSTEM",Url = "/system/role"},
                    new Function {Id = "SYSTEM_FUNCTION", Description="Chức năng",ParentId = "SYSTEM",Url = "/system/function"},
                    new Function {Id = "SYSTEM_PERMISSION", Description="Quyền hạn",ParentId = "SYSTEM",Url = "/system/permission"},
                });
                await _context.SaveChangesAsync();
            }

            if (!_context.Commands.Any())
            {
                _context.Commands.AddRange(new List<Command>()
                {
                    new Command(){Id = nameof(MyApplicationDefine.VIEW), Name = MyApplicationDefine.VIEW},
                    new Command(){Id = nameof(MyApplicationDefine.CREATE), Name = MyApplicationDefine.CREATE},
                    new Command(){Id = nameof(MyApplicationDefine.UPDATE), Name = MyApplicationDefine.UPDATE},
                    new Command(){Id = nameof(MyApplicationDefine.DELETE), Name = MyApplicationDefine.DELETE},
                    new Command(){Id = nameof(MyApplicationDefine.APPROVE), Name = MyApplicationDefine.APPROVE},
                });
            }
            #endregion

            var functions = _context.Functions;

            if (!_context.CommandInFunctions.Any())
            {
                foreach (var function in functions)
                {
                    var createAction = new CommandInFunction()
                    {
                        CommandId = nameof(MyApplicationDefine.CREATE),
                        FunctionId = function.Id
                    };
                    _context.CommandInFunctions.Add(createAction);

                    var updateAction = new CommandInFunction()
                    {
                        CommandId = nameof(MyApplicationDefine.UPDATE),
                        FunctionId = function.Id
                    };
                    _context.CommandInFunctions.Add(updateAction);
                    var deleteAction = new CommandInFunction()
                    {
                        CommandId = nameof(MyApplicationDefine.DELETE),
                        FunctionId = function.Id
                    };
                    _context.CommandInFunctions.Add(deleteAction);

                    var viewAction = new CommandInFunction()
                    {
                        CommandId = nameof(MyApplicationDefine.VIEW),
                        FunctionId = function.Id
                    };
                    _context.CommandInFunctions.Add(viewAction);
                }
            }

            if (!_context.Permissions.Any())
            {
                var adminRole = await _roleManager.FindByNameAsync(MyApplicationDefine.Admin);
                foreach (var function in functions)
                {
                    _context.Permissions.Add(new Permission { RoleId = adminRole.Id, FunctionId = function.Id, CommandId = nameof(MyApplicationDefine.CREATE)});
                    _context.Permissions.Add(new Permission { RoleId = adminRole.Id, FunctionId = function.Id, CommandId = nameof(MyApplicationDefine.UPDATE) });
                    _context.Permissions.Add(new Permission { RoleId = adminRole.Id, FunctionId = function.Id, CommandId = nameof(MyApplicationDefine.DELETE)});
                    _context.Permissions.Add(new Permission { RoleId = adminRole.Id, FunctionId = function.Id, CommandId = nameof(MyApplicationDefine.VIEW)});
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
