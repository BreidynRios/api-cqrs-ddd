using Application.Commons.Exceptions;
using Application.Features.PermissionTypes.Queries.GetPermissionTypeById;
using Application.Test.Configurations.AutoMoq;
using AutoFixture.Xunit2;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Test.Features.PermissionTypes.Queries.GetPermissionTypeById
{
    public class GetPermissionTypeByIdQueryHandlerTest
    {
        [Theory(DisplayName = "When permission type don't exist, it will return an error"), AutoMoq]
        public async Task Handle_NotFound(
            [Frozen] Mock<IPermissionTypeRepository> mockIPermissionTypeRepository,
            GetPermissionTypeByIdQuery request,
            GetPermissionTypeByIdQueryHandler sut)
        {
            //ARRANGE
            const string errorMessage = "Permission type wasn't found";
            mockIPermissionTypeRepository.Setup(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as PermissionType);

            //ACT
            var actual = async () => await sut.Handle(request, CancellationToken.None);

            //ASSERT
            await actual.Should()
                .ThrowAsync<NotFoundException>()
                .Where(m => m.Message == errorMessage);
            mockIPermissionTypeRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When permission type exist, it will return his information"), AutoMoq]
        public async Task Handle_Ok(
            PermissionType permissionType,
            [Frozen] Mock<IPermissionTypeRepository> mockIPermissionTypeRepository,
            GetPermissionTypeByIdQuery request,
            GetPermissionTypeByIdQueryHandler sut)
        {
            //ARRANGE
            mockIPermissionTypeRepository.Setup(x => x.GetByIdAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(permissionType);

            //ACT
            var actual = await sut.Handle(request, CancellationToken.None);

            //ASSERT
            actual.Should().NotBeNull();
            mockIPermissionTypeRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
