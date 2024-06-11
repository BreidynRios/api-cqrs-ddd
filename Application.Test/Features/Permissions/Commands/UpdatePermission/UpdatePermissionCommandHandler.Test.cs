using Application.Commons.Exceptions;
using Application.DTOs.ServicesClients.ElasticSearch;
using Application.Features.Permissions.Commands.UpdatePermission;
using Application.Interfaces.ServicesClients;
using Application.Test.Configurations.AutoMoq;
using AutoFixture.Xunit2;
using Domain.Entities;
using Domain.Repositories;
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
            [Frozen] Mock<IPermissionRepository> mockIPermissionRepository,
            UpdatePermissionCommand request,
            [Frozen] Mock<UpdatePermissionCommandHandler> sutMock)
        {
            //ARRANGE
            sutMock.Setup(x => x.Validate(It.IsAny<UpdatePermissionCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            mockIPermissionRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(permission);

            sutMock.Setup(x => x.AssignPermission(It.IsAny<Permission>(), It.IsAny<UpdatePermissionCommand>()));

            mockIPermissionRepository.Setup(x => x.Update(It.IsAny<Permission>()));

            sutMock.Setup(x => x.ElasticSearchCreateDocument(It.IsAny<Permission>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            //ACT
            await sutMock.Object.Handle(request, CancellationToken.None);

            //ASSERT
            sutMock.Verify(x => x.Validate(It.IsAny<UpdatePermissionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mockIPermissionRepository.Verify(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            sutMock.Verify(x => x.AssignPermission(It.IsAny<Permission>(), It.IsAny<UpdatePermissionCommand>()), Times.Once);
            mockIPermissionRepository.Verify(x => x.Update(It.IsAny<Permission>()), Times.Once);
            sutMock.Verify(x => x.ElasticSearchCreateDocument(
                It.IsAny<Permission>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region Validate

        [Theory(DisplayName = "When employee don't exist, it will return an error"), AutoMoq]
        public async Task Validate_Employee_NotFound(
            [Frozen] Mock<IEmployeeRepository> mockIEmployeeRepository,
            UpdatePermissionCommand request,
            UpdatePermissionCommandHandler sut)
        {
            //ARRANGE
            const string errorMessage = "Employee wasn't found";
            mockIEmployeeRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as Employee);

            //ACT
            var actual = async () => await sut.Validate(request, CancellationToken.None);

            //ASSERT
            await actual.Should()
                .ThrowAsync<NotFoundException>()
                .Where(m => m.Message == errorMessage);
            mockIEmployeeRepository.Verify(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When permission type don't exist, it will return an error"), AutoMoq]
        public async Task Validate_PermissionType_NotFound(
            Employee employee,
            [Frozen] Mock<IEmployeeRepository> mockIEmployeeRepository,
            [Frozen] Mock<IPermissionTypeRepository> mockIPermissionTypeRepository,
            UpdatePermissionCommand request,
            UpdatePermissionCommandHandler sut)
        {
            //ARRANGE
            const string errorMessage = "Permission type wasn't found";
            mockIEmployeeRepository.Setup(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            mockIPermissionTypeRepository.Setup(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as PermissionType);

            //ACT
            var actual = async () => await sut.Validate(request, CancellationToken.None);

            //ASSERT
            await actual.Should()
                .ThrowAsync<NotFoundException>()
                .Where(m => m.Message == errorMessage);
            mockIEmployeeRepository.Verify(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            mockIPermissionTypeRepository.Verify(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When employee and permission type exist, it will not return an error"), AutoMoq]
        public async Task Validate_Ok(
            Employee employee,
            PermissionType permissionType,
            [Frozen] Mock<IEmployeeRepository> mockIEmployeeRepository,
            [Frozen] Mock<IPermissionTypeRepository> mockIPermissionTypeRepository,
            UpdatePermissionCommand request,
            UpdatePermissionCommandHandler sut)
        {
            //ARRANGE
            mockIEmployeeRepository.Setup(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            mockIPermissionTypeRepository.Setup(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(permissionType);

            //ACT
            await sut.Validate(request, CancellationToken.None);

            //ASSERT
            mockIEmployeeRepository.Verify(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            mockIPermissionTypeRepository.Verify(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
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
            permission.UpdatedDateOnUtc.Should().NotBe(null);
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
