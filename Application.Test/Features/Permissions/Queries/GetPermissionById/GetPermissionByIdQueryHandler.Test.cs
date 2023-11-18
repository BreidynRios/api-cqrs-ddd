using Application.Commons.Exceptions;
using Application.DTOs.ServicesClients.ElasticSearch;
using Application.Features.Permissions.Queries.GetPermissionById;
using Application.Interfaces.Common;
using Application.Interfaces.ServicesClients;
using Application.Test.Configurations.AutoMoq;
using AutoFixture.Xunit2;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Test.Features.Permissions.Queries.GetPermissionById
{
    public class GetPermissionByIdQueryHandlerTest
    {
        #region Handle

        [Theory(DisplayName = "When permission don't exist, it will return an error"), AutoMoq]
        public async Task Handle_NotFound(
            [Frozen] Mock<IUnitOfWork> mockIUnitOfWork,
            GetPermissionByIdQuery request,
            GetPermissionByIdQueryHandler sut)
        {
            //ARRANGE
            mockIUnitOfWork.Setup(x => x.PermissionRepository
                .GetPermissionWithEmployeeTypeAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as Permission);

            //ACT
            var actual = async () => await sut.Handle(request, CancellationToken.None);

            //ASSERT
            await actual.Should()
                .ThrowAsync<NotFoundException>()
                .Where(m => m.Message.Contains(request.Id.ToString()));
            mockIUnitOfWork.Verify(x => x.PermissionRepository
                .GetPermissionWithEmployeeTypeAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When permission exist, it will return his information"), AutoMoq]
        public async Task Handle_Ok(
            Permission permission,
            [Frozen] Mock<IUnitOfWork> mockIUnitOfWork,
            GetPermissionByIdQuery request,
            [Frozen] Mock<GetPermissionByIdQueryHandler> sutMock)
        {
            //ARRANGE
            mockIUnitOfWork.Setup(x => x.PermissionRepository
                .GetPermissionWithEmployeeTypeAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(permission);

            sutMock.Setup(x => x.ElasticSearchCreateDocument(It.IsAny<Permission>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            //ACT
            var actual = await sutMock.Object.Handle(request, CancellationToken.None);

            //ASSERT
            actual.Should().NotBeNull();
            mockIUnitOfWork.Verify(x => x.PermissionRepository
                .GetPermissionWithEmployeeTypeAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            sutMock.Verify(x => x.ElasticSearchCreateDocument(
                It.IsAny<Permission>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region ElasticSearchCreateDocument

        [Theory(DisplayName = "When permission employee was obtained, it will call to elasticsearch"), AutoMoq]
        public async Task ElasticSearchCreateDocument_Ok(
            Permission permission,
            [Frozen] Mock<IElasticSearchServiceClient> mockIElasticSearchServiceClient,
            GetPermissionByIdQueryHandler sut)
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
