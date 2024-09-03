using Application.DTOs.Response;
using Application.Features.Permissions.Commands.CreatePermission;
using Application.Features.Permissions.Commands.DeletePermission;
using Application.Features.Permissions.Commands.UpdatePermission;
using Application.Features.Permissions.Queries.GetPermissionById;
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
    public class PermissionsControllerTest
    {
        #region GetPermissionByIdQueryAsync

        [Theory(DisplayName = "It should return the permission"), AutoMoq]
        public async Task GetPermissionByIdQueryAsync_Ok(
            int id,
            GetPermissionByIdDto permission,
            [Frozen] Mock<IMediator> mockIMediator,
            PermissionsController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<GetPermissionByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<GetPermissionByIdDto>.Success(permission));

            //ACT
            var actual = await sut.GetPermissionByIdQueryAsync(id, CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<OkObjectResult>();
            var result = actual.Result as OkObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            result!.Value.Should().Be(permission);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<GetPermissionByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When permission doesn't exist it returns not found"), AutoMoq]
        public async Task GetPermissionByIdQueryAsync_NotFound(
            string errorMessage,
            [Frozen] Mock<IMediator> mockIMediator,
            PermissionsController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<GetPermissionByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<GetPermissionByIdDto>.Failure(errorMessage));

            //ACT
            var actual = await sut.GetPermissionByIdQueryAsync(default, CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<NotFoundObjectResult>();
            var result = actual.Result as NotFoundObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result!.Value.Should().Be(errorMessage);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<GetPermissionByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region CreateAsync

        [Theory(DisplayName = "It should return the new permission Id"), AutoMoq]
        public async Task CreateAsync_Ok(
           int id,
           [Frozen] Mock<IMediator> mockIMediator,
           CreatePermissionCommand request,
           PermissionsController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<CreatePermissionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<int>.Success(id));

            //ACT
            var actual = await sut.CreateAsync(request, CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<OkObjectResult>();
            var result = actual.Result as OkObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            result!.Value.Should().Be(id);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<CreatePermissionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When there are errors it returns not found"), AutoMoq]
        public async Task CreateAsync_NotFound(
           string errorMessage,
           [Frozen] Mock<IMediator> mockIMediator,
           CreatePermissionCommand request,
           PermissionsController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<CreatePermissionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<int>.Failure(errorMessage));

            //ACT
            var actual = await sut.CreateAsync(request, CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<NotFoundObjectResult>();
            var result = actual.Result as NotFoundObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result!.Value.Should().Be(errorMessage);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<CreatePermissionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region UpdateAsync

        [Theory(DisplayName = "It should update the permission"), AutoMoq]
        public async Task UpdateAsync_Ok(
            int id,
            [Frozen] Mock<IMediator> mockIMediator,
            UpdatePermissionCommand request,
            PermissionsController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<UpdatePermissionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success());

            //ACT
            var actual = await sut.UpdateAsync(id, request, CancellationToken.None);

            //ASSERT
            actual.Should().BeOfType<OkResult>();
            var result = actual as OkResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<UpdatePermissionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When the permission doesn't exist it returns not found"), AutoMoq]
        public async Task UpdateAsync_NotFound(
            string errorMessage,
            [Frozen] Mock<IMediator> mockIMediator,
            UpdatePermissionCommand request,
            PermissionsController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<UpdatePermissionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure(errorMessage));

            //ACT
            var actual = await sut.UpdateAsync(default, request, CancellationToken.None);

            //ASSERT
            actual.Should().BeOfType<NotFoundObjectResult>();
            var result = actual as NotFoundObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result!.Value.Should().Be(errorMessage);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<UpdatePermissionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region DeleteAsync

        [Theory(DisplayName = "It should delete the permission"), AutoMoq]
        public async Task DeleteAsync_Ok(
            int id,
            [Frozen] Mock<IMediator> mockIMediator,
            PermissionsController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<DeletePermissionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Success());

            //ACT
            var actual = await sut.DeleteAsync(id, CancellationToken.None);

            //ASSERT
            actual.Should().BeOfType<OkResult>();
            var result = actual as OkResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<DeletePermissionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When the permission to delete doesn't exist it returns not found"), AutoMoq]
        public async Task DeleteAsync_NotFound(
            string errorMessage,
            [Frozen] Mock<IMediator> mockIMediator,
            PermissionsController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<DeletePermissionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure(errorMessage));

            //ACT
            var actual = await sut.DeleteAsync(default, CancellationToken.None);

            //ASSERT
            actual.Should().BeOfType<NotFoundObjectResult>();
            var result = actual as NotFoundObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result!.Value.Should().Be(errorMessage);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<DeletePermissionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
    }
}
