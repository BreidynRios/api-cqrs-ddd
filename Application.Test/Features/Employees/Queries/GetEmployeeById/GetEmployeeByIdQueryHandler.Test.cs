using Application.Features.Employees.Queries.GetEmployeeById;
using Application.Test.Configurations.AutoMoq;
using AutoFixture.Xunit2;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Test.Features.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandlerTest
    {
        [Theory(DisplayName = "When employee don't exist, it will return an error"), AutoMoq]
        public async Task Handle_NotFound(
            [Frozen] Mock<IEmployeeRepository> mockIEmployeeRepository,
            GetEmployeeByIdQuery request,
            GetEmployeeByIdQueryHandler sut)
        {
            //ARRANGE
            const string errorMessage = "Employee wasn't found";
            mockIEmployeeRepository.Setup(x => x.GetEmployeeWithPermissionsAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as Employee);

            //ACT
            var actual = await sut.Handle(request, CancellationToken.None);

            //ASSERT
            actual.ErrorMessage.Should().Be(errorMessage);
            mockIEmployeeRepository.Verify(x => x.GetEmployeeWithPermissionsAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When employee exist, it will return his information"), AutoMoq]
        public async Task Handle_Ok(
            Employee employee,
            [Frozen] Mock<IEmployeeRepository> mockIEmployeeRepository,
            GetEmployeeByIdQuery request,
            GetEmployeeByIdQueryHandler sut)
        {
            //ARRANGE
            mockIEmployeeRepository.Setup(x => x.GetEmployeeWithPermissionsAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            //ACT
            var actual = await sut.Handle(request, CancellationToken.None);

            //ASSERT
            actual.Should().NotBeNull();
            mockIEmployeeRepository.Verify(x => x.GetEmployeeWithPermissionsAsync(
                It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
