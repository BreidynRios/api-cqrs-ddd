using Application.Commons.Exceptions;
using Application.Features.Employees.Queries.GetEmployeeById;
using Application.Interfaces.Common;
using Application.Test.Configurations.AutoMoq;
using AutoFixture.Xunit2;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Test.Features.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandlerTest
    {
        [Theory(DisplayName = "When employee don't exist, it will return an error"), AutoMoq]
        public async Task Handle_NotFound(
            [Frozen] Mock<IUnitOfWork> mockIUnitOfWork,
            GetEmployeeByIdQuery request,
            GetEmployeeByIdQueryHandler sut)
        {
            //ARRANGE
            mockIUnitOfWork.Setup(x => x.EmployeeRepository
                .GetEmployeeWithPermissionsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as Employee);

            //ACT
            var actual = async () => await sut.Handle(request, CancellationToken.None);

            //ASSERT
            await actual.Should()
                .ThrowAsync<NotFoundException>()
                .Where(m => m.Message.Contains(request.Id.ToString()));
            mockIUnitOfWork.Verify(x => x.EmployeeRepository
                .GetEmployeeWithPermissionsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When employee exist, it will return his information"), AutoMoq]
        public async Task Handle_Ok(
            Employee employee,
            [Frozen] Mock<IUnitOfWork> mockIUnitOfWork,
            GetEmployeeByIdQuery request,
            GetEmployeeByIdQueryHandler sut)
        {
            //ARRANGE
            mockIUnitOfWork.Setup(x => x.EmployeeRepository
                .GetEmployeeWithPermissionsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            //ACT
            var actual = await sut.Handle(request, CancellationToken.None);

            //ASSERT
            actual.Should().NotBeNull();
            mockIUnitOfWork.Verify(x => x.EmployeeRepository
                .GetEmployeeWithPermissionsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
