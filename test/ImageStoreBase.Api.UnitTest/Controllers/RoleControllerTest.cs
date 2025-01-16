using AutoMapper;
using ImageStoreBase.Api.Controllers;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.Roles;
using ImageStoreBase.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MockQueryable;
using Moq;

namespace ImageStoreBase.Api.UnitTest.Controllers
{
    public class RoleControllerTest
    {
        //private readonly Mock<RoleManager<Role>> _mockRoleManager;
        //private readonly Mock<IMapper> _mockMapper;
        //private readonly IRoleService _roleService;
        //private readonly RolesController _controller;

        //public RoleControllerTest()
        //{
        //    // Thiết lập mock cho RoleManager
        //    var roleStore = new Mock<IRoleStore<Role>>();
        //    _mockRoleManager = new Mock<RoleManager<Role>>(
        //        roleStore.Object,
        //        null,
        //        null,
        //        null,
        //        null
        //    );

        //    // Thiết lập mock cho IMapper
        //    _mockMapper = new Mock<IMapper>();

        //    // Tạo instance thực của RoleService với các dependencies được mock
        //    _roleService = new RoleService(_mockRoleManager.Object, _mockMapper.Object);

        //    // Tạo controller với RoleService thực
        //    _controller = new RoleController(_roleService);
        //}

        //[Fact]
        //public void ShouldCreateController_NotNull_Success()
        //{
        //    var rolesController = new RoleController(_roleService);
        //    Assert.NotNull(rolesController);
        //}

        //[Fact]
        //public async Task GetAllRoles_ReturnsOkResult_WithListOfRoles()
        //{
        //    // Arrange
        //    var expectedRoles = new List<RoleResponse>
        //    {
        //        new RoleResponse { Id = "1", Name = "Admin" },
        //        new RoleResponse { Id = "2", Name = "User" }
        //    };
        //    _mockRoleManager.Setup(x => x.Roles)
        //    .Returns(new List<Role>
        //    {
        //        new Role { Id = Guid.NewGuid(), Name = "Admin" },
        //        new Role { Id = Guid.NewGuid(), Name = "User" }
        //    }.BuildMock());
        //    _mockMapper.Setup(m => m.Map<List<RoleResponse>>(It.IsAny<IEnumerable<Role>>()))
        //        .Returns(expectedRoles);

        //    // Act
        //    var result = await _controller.GetAllRoles();

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result.Result);
        //    var returnedRoles = Assert.IsType<List<RoleResponse>>(okResult.Value);
        //    Assert.Equal(2, returnedRoles.Count);
        //    Assert.Equal("Admin", returnedRoles[0].Name);
        //    Assert.Equal("User", returnedRoles[1].Name);
        //}

        //[Fact]
        //public async Task GetRoleById_WithValidId_ReturnsOkResult()
        //{
        //    // Arrange
        //    var roleId = "1";
        //    var expectedRole = new RoleResponse { Id = roleId, Name = "Admin" };
        //    _mockRoleManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
        //        .ReturnsAsync(new Role { Id = Guid.NewGuid(), Name = "Admin" });
        //    _mockMapper.Setup(m => m.Map<RoleResponse>(It.IsAny<Role>()))
        //        .Returns(expectedRole);

        //    // Act
        //    var result = await _controller.GetRoleById(roleId);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result.Result);
        //    var returnedRole = Assert.IsType<RoleResponse>(okResult.Value);
        //    Assert.Equal("Admin", returnedRole.Name);
        //}

        //[Fact]
        //public async Task CreateRole_WithValidName_ReturnsCreatedAtAction()
        //{
        //    // Arrange
        //    var roleName = "NewRole";
        //    var createdRole = new Role { Id = Guid.NewGuid(), Name = roleName };
        //    _mockRoleManager.Setup(x => x.CreateAsync(It.IsAny<Role>()))
        //        .ReturnsAsync(IdentityResult.Success);
        //    // Act
        //    var result = await _controller.CreateRole(roleName);

        //    // Assert
        //    var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        //    var returnedRole = Assert.IsType<Role>(createdAtActionResult.Value);
        //    Assert.Equal(roleName, returnedRole.Name);
        //}

        //[Fact]
        //public async Task DeleteRole_WithValidId_ReturnsNoContent()
        //{
        //    // Arrange
        //    var roleId = Guid.NewGuid();
        //    _mockRoleManager.Setup(i => i.FindByIdAsync(It.IsAny<string>()))
        //        .ReturnsAsync(new Role
        //        {
        //            Id = roleId,
        //            Name = "Task cần xóa"
        //        });
        //    _mockRoleManager.Setup(i => i.DeleteAsync(It.IsAny<Role>()))
        //        .ReturnsAsync(IdentityResult.Success);

        //    // Act
        //    var result = await _controller.DeleteRole(roleId.ToString());

        //    // Assert
        //    Assert.IsType<NoContentResult>(result);
        //}

        //[Fact]
        //public async Task UpdateRole_WithValidData_ReturnsOkResult()
        //{
        //    // Arrange
        //    var roleId = Guid.NewGuid();
        //    var newRoleName = "UpdatedRole";
        //    var updatedRole = new Role { Id = roleId, Name = newRoleName };

        //    _mockRoleManager.Setup(i => i.FindByIdAsync(It.IsAny<string>()))
        //        .ReturnsAsync(new Role
        //        {
        //            Id = roleId,
        //            Name = "Task cần update"
        //        });
        //    _mockRoleManager.Setup(i => i.UpdateAsync(It.IsAny<Role>()))
        //        .ReturnsAsync(IdentityResult.Success);
        //    // Act
        //    var result = await _controller.UpdateRole(roleId, newRoleName);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var returnedRole = Assert.IsType<Role>(okResult.Value);
        //    Assert.Equal(newRoleName, returnedRole.Name);
        //}
    }
}
