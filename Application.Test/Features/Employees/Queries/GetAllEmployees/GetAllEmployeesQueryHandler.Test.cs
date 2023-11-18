using Application.Features.Employees.Queries.GetAllEmployees;
using Application.Interfaces.Common;
using Application.Test.Configurations.AutoMoq;
using AutoFixture.Xunit2;
using Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Test.Features.Employees.Queries.GetAllEmployees
{
    public class GetAllEmployeesQueryHandlerTest
    {
        [Theory(DisplayName = "When data exists, it will return the list"), AutoMoq]
        public async Task Handle_Ok(
            [CollectionSize(5)] List<Employee> employees,
            [Frozen] Mock<IUnitOfWork> mockIUnitOfWork,
            GetAllEmployeesQuery request,
            GetAllEmployeesQueryHandler sut)
        {
            //ARRANGE
            mockIUnitOfWork.Setup(x => x.EmployeeRepository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(employees);

            //ACT
            var actual = await sut.Handle(request, CancellationToken.None);

            //ASSERT
            actual.Should().NotBeNull();
            actual.Should().HaveCount(5);
            mockIUnitOfWork.Verify(x => x.EmployeeRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
