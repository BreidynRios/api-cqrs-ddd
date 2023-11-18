using Application.Features.PermissionTypes.Queries.GetAllPermissionTypes;
using Application.Interfaces.Common;
using Application.Test.Configurations.AutoMoq;
using AutoFixture.Xunit2;
using Domain.Entities;
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
            [Frozen] Mock<IUnitOfWork> mockIUnitOfWork,
            GetAllPermissionTypesQuery request,
            GetAllPermissionTypesQueryHandler sut)
        {
            //ARRANGE
            mockIUnitOfWork.Setup(x => x.PermissionTypeRepository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(permissionTypes);

            //ACT
            var actual = await sut.Handle(request, CancellationToken.None);

            //ASSERT
            actual.Should().NotBeNull();
            actual.Should().HaveCount(5);
            mockIUnitOfWork.Verify(x => x.PermissionTypeRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
