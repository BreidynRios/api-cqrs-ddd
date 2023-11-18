using Application.Features.PermissionTypes.Queries.GetAllPermissionTypes;
using Application.Features.PermissionTypes.Queries.GetPermissionTypeById;
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
    public class PermissionTypesControllerTest
    {
        [Theory(DisplayName = "It should return the list of permission types"), AutoMoq]
        public async Task GetAsync_Ok(
            [CollectionSize(5)] List<GetAllPermissionTypesDto> permissionTypes,
            [Frozen] Mock<IMediator> mockIMediator,
            PermissionTypesController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<GetAllPermissionTypesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(permissionTypes);

            //ACT
            var actual = await sut.GetAsync();

            //ASSERT
            actual.Result.Should().BeOfType<OkObjectResult>();
            var result = actual.Result as OkObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().Be(permissionTypes);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<GetAllPermissionTypesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "It should return the permission type"), AutoMoq]
        public async Task GetPermissionTypeByIdAsync_Ok(
            GetPermissionTypeByIdDto permissionType,
            [Frozen] Mock<IMediator> mockIMediator,
            PermissionTypesController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<GetPermissionTypeByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(permissionType);

            //ACT
            var actual = await sut.GetPermissionTypeByIdAsync(permissionType.Id);

            //ASSERT
            actual.Result.Should().BeOfType<OkObjectResult>();
            var result = actual.Result as OkObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().Be(permissionType);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<GetPermissionTypeByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
