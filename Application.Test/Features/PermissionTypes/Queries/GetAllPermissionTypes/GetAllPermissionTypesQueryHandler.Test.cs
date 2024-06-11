using Application.Features.PermissionTypes.Queries.GetAllPermissionTypes;
using Application.Test.Configurations.AutoMoq;
using AutoFixture.Xunit2;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Test.Features.PermissionTypes.Queries.GetAllPermissionTypes
{
    public class GetAllPermissionTypesQueryHandlerTest
    {
        [Theory(DisplayName = "When data exists, it will return the list"), AutoMoq]
        public async Task Handle_Ok(
            [CollectionSize(5)] List<PermissionType> permissionTypes,
            [Frozen] Mock<IPermissionTypeRepository> mockIPermissionTypeRepository,
            GetAllPermissionTypesQuery request,
            GetAllPermissionTypesQueryHandler sut)
        {
            //ARRANGE
            mockIPermissionTypeRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(permissionTypes);

            //ACT
            var actual = await sut.Handle(request, CancellationToken.None);

            //ASSERT
            actual.Should().NotBeNull();
            actual.Should().HaveCount(5);
            mockIPermissionTypeRepository.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
