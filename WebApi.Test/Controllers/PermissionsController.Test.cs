using Application.Features.Permissions.Commands.CreatePermission;
using Application.Features.Permissions.Commands.UpdatePermission;
using Application.Features.Permissions.Queries.GetPermissionById;
using Application.Test.Configurations.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using Xunit;

namespace WebApi.Test.Controllers
{
    public class PermissionsControllerTest
    {
        [Theory(DisplayName = "It should return the permission"), AutoMoq]
        public async Task GetPermissionByIdQueryAsync_Ok(
            int id,
            GetPermissionByIdDto permission,
            [Frozen] Mock<IMediator> mockIMediator,
            PermissionsController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<GetPermissionByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(permission);

            //ACT
            var actual = await sut.GetPermissionByIdQueryAsync(id, CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<OkObjectResult>();
            var result = actual.Result as OkObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().Be(permission);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<GetPermissionByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "It should return the new permission Id"), AutoMoq]
        public async Task CreateAsync_Ok(
            int id,
            [Frozen] Mock<IMediator> mockIMediator,
            CreatePermissionCommand request,
            PermissionsController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<CreatePermissionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(id);

            //ACT
            var actual = await sut.CreateAsync(request, CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<OkObjectResult>();
            var result = actual.Result as OkObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().Be(id);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<CreatePermissionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "It should update the permission"), AutoMoq]
        public async Task UpdateAsync_Ok(
            int id,
            [Frozen] Mock<IMediator> mockIMediator,
            UpdatePermissionCommand request,
            PermissionsController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<UpdatePermissionCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            //ACT
            await sut.UpdateAsync(id, request, CancellationToken.None);

            //ASSERT
            request.Id.Should().Be(id);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<UpdatePermissionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
