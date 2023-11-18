using Application.Commons.Exceptions;
using Application.DTOs.ServicesClients.ElasticSearch;
using Application.Features.Permissions.Commands.UpdatePermission;
using Application.Interfaces.Common;
using Application.Interfaces.ServicesClients;
using Application.Test.Configurations.AutoMoq;
using AutoFixture.Xunit2;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Test.Features.Permissions.Commands.UpdatePermission
{
    public class UpdatePermissionCommandHandlerTest
    {
        #region Handle

        [Theory(DisplayName = "When the process is successful, it will return the id"), AutoMoq]
        public async Task Handle_Ok(
            Permission permission,
            [Frozen] Mock<IUnitOfWork> mockIUnitOfWork,
            UpdatePermissionCommand request,
            [Frozen] Mock<UpdatePermissionCommandHandler> sutMock)
        {
            //ARRANGE
            sutMock.Setup(x => x.Validate(It.IsAny<UpdatePermissionCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mockIUnitOfWork.Setup(x => x.PermissionRepository
                .GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(permission);

            sutMock.Setup(x => x.AssignPermission(It.IsAny<Permission>(), It.IsAny<UpdatePermissionCommand>()));

            mockIUnitOfWork.Setup(x => x.PermissionRepository
                .UpdateAsync(It.IsAny<Permission>()))
                .Returns(Task.CompletedTask);

            sutMock.Setup(x => x.ElasticSearchCreateDocument(It.IsAny<Permission>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            //ACT
            await sutMock.Object.Handle(request, CancellationToken.None);

            //ASSERT
            sutMock.Verify(x => x.Validate(It.IsAny<UpdatePermissionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mockIUnitOfWork.Verify(x => x.PermissionRepository
                .GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            sutMock.Verify(x => x.AssignPermission(It.IsAny<Permission>(), It.IsAny<UpdatePermissionCommand>()), Times.Once);
            mockIUnitOfWork.Verify(x => x.PermissionRepository
                .UpdateAsync(It.IsAny<Permission>()), Times.Once);
            sutMock.Verify(x => x.ElasticSearchCreateDocument(
                It.IsAny<Permission>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Validate

        [Theory(DisplayName = "When employee don't exist, it will return an error"), AutoMoq]
        public async Task Validate_Employee_NotFound(
            [Frozen] Mock<IUnitOfWork> mockIUnitOfWork,
            UpdatePermissionCommand request,
            UpdatePermissionCommandHandler sut)
        {
            //ARRANGE
            mockIUnitOfWork.Setup(x => x.EmployeeRepository
                .GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as Employee);

            //ACT
            var actual = async () => await sut.Validate(request, CancellationToken.None);

            //ASSERT
            await actual.Should()
                .ThrowAsync<NotFoundException>()
                .Where(m => m.Message.Contains(request.EmployeeId.ToString()));
            mockIUnitOfWork.Verify(x => x.EmployeeRepository
                .GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When permission type don't exist, it will return an error"), AutoMoq]
        public async Task Validate_PermissionType_NotFound(
            Employee employee,
            [Frozen] Mock<IUnitOfWork> mockIUnitOfWork,
            UpdatePermissionCommand request,
            UpdatePermissionCommandHandler sut)
        {
            //ARRANGE
            mockIUnitOfWork.Setup(x => x.EmployeeRepository
                .GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            mockIUnitOfWork.Setup(x => x.PermissionTypeRepository
                .GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as PermissionType);

            //ACT
            var actual = async () => await sut.Validate(request, CancellationToken.None);

            //ASSERT
            await actual.Should()
                .ThrowAsync<NotFoundException>()
                .Where(m => m.Message.Contains(request.PermissionTypeId.ToString()));
            mockIUnitOfWork.Verify(x => x.EmployeeRepository
                .GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            mockIUnitOfWork.Verify(x => x.PermissionTypeRepository
                .GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When employee and permission type exist, it will not return an error"), AutoMoq]
        public async Task Validate_Ok(
            Employee employee,
            PermissionType permissionType,
            [Frozen] Mock<IUnitOfWork> mockIUnitOfWork,
            UpdatePermissionCommand request,
            UpdatePermissionCommandHandler sut)
        {
            //ARRANGE
            mockIUnitOfWork.Setup(x => x.EmployeeRepository
                .GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            mockIUnitOfWork.Setup(x => x.PermissionTypeRepository
                .GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(permissionType);

            //ACT
            await sut.Validate(request, CancellationToken.None);

            //ASSERT
            mockIUnitOfWork.Verify(x => x.EmployeeRepository
                .GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            mockIUnitOfWork.Verify(x => x.PermissionTypeRepository
                .GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region AssignPermission

        [Theory(DisplayName = "When assign the data, it will return an entity permission"), AutoMoq]
        public void AssignPermission_Ok(
            Permission permission,
            UpdatePermissionCommand request,
            UpdatePermissionCommandHandler sut)
        {
            //ACT
            sut.AssignPermission(permission, request);

            //ASSERT
            permission.Should().NotBeNull();
            permission.EmployeeId.Should().Be(request.EmployeeId);
            permission.PermissionTypeId.Should().Be(request.PermissionTypeId);
            permission.UpdatedBy.Should().NotBe(null);
            permission.UpdatedDate.Should().NotBe(null);
        }

        #endregion

        #region ElasticSearchCreateDocument

        [Theory(DisplayName = "When permission employee was updated, it will call to elasticsearch"), AutoMoq]
        public async Task ElasticSearchCreateDocument_Ok(
            Permission permission,
            [Frozen] Mock<IElasticSearchServiceClient> mockIElasticSearchServiceClient,
            UpdatePermissionCommandHandler sut)
        {
            //ARRANGE
            mockIElasticSearchServiceClient.Setup(x => x
                .CreateDocumentAsync(It.IsAny<PermissionParameter>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            //ACT
            await sut.ElasticSearchCreateDocument(permission, CancellationToken.None);

            //ASSERT
            mockIElasticSearchServiceClient.Verify(x => x.CreateDocumentAsync(
                It.IsAny<PermissionParameter>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
    }
}
