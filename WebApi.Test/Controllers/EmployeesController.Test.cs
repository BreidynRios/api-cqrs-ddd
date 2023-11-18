using Application.Features.Employees.Queries.GetAllEmployees;
using Application.Features.Employees.Queries.GetEmployeeById;
using Application.Test.Configurations.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using Xunit;

namespace WebApi.Test.Controllers
{
    public class EmployeesControllerTest
    {
        [Theory(DisplayName = "It should return the list of employees"), AutoMoq]
        public async Task GetAsync_Ok(
            [CollectionSize(5)] List<GetAllEmployeesDto> employees,
            [Frozen] Mock<IMediator> mockIMediator,
            EmployeesController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<GetAllEmployeesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(employees);

            //ACT
            var actual = await sut.GetAsync();

            //ASSERT
            actual.Result.Should().BeOfType<OkObjectResult>();
            var result = actual.Result as OkObjectResult;
            result.Should().NotBeNull();
            result.Value.Should().Be(employees);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<GetAllEmployeesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "It should return the employee"), AutoMoq]
        public async Task GetEmployeeByIdAsync_Ok(
            GetEmployeeByIdDto employee,
            [Frozen] Mock<IMediator> mockIMediator,
            EmployeesController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<GetEmployeeByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(employee);

            //ACT
            var actual = await sut.GetEmployeeByIdAsync(employee.Id);

            //ASSERT
            actual.Result.Should().BeOfType<OkObjectResult>();
            var result = actual.Result as OkObjectResult;
            result.Should().NotBeNull();            
            result.Value.Should().Be(employee);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<GetEmployeeByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
