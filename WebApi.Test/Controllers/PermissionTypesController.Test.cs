using Application.DTOs.Response;
using Application.Features.PermissionTypes.Queries.GetAllPermissionTypes;
using Application.Features.PermissionTypes.Queries.GetPermissionTypeById;
using Application.Test.Configurations.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using Xunit;

namespace WebApi.Test.Controllers
{
    public class PermissionTypesControllerTest
    {
        #region GetAsync

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
            var actual = await sut.GetAsync(CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<OkObjectResult>();
            var result = actual.Result as OkObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            result!.Value.Should().Be(permissionTypes);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<GetAllPermissionTypesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region GetPermissionTypeByIdAsync

        [Theory(DisplayName = "It should return the permission type"), AutoMoq]
        public async Task GetPermissionTypeByIdAsync_Ok(
            GetPermissionTypeByIdDto permissionType,
            [Frozen] Mock<IMediator> mockIMediator,
            PermissionTypesController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<GetPermissionTypeByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<GetPermissionTypeByIdDto>.Success(permissionType));

            //ACT
            var actual = await sut.GetPermissionTypeByIdAsync(permissionType.Id, CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<OkObjectResult>();
            var result = actual.Result as OkObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            result!.Value.Should().Be(permissionType);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<GetPermissionTypeByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When permission type doesn't exist it returns not found"), AutoMoq]
        public async Task GetPermissionTypeByIdAsync_NotFound(
            string errorMessage,
            [Frozen] Mock<IMediator> mockIMediator,
            PermissionTypesController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<GetPermissionTypeByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<GetPermissionTypeByIdDto>.Failure(errorMessage));

            //ACT
            var actual = await sut.GetPermissionTypeByIdAsync(default, CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<NotFoundObjectResult>();
            var result = actual.Result as NotFoundObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result!.Value.Should().Be(errorMessage);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<GetPermissionTypeByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
    }
}
