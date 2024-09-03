using Application.DTOs.Response;
using Application.Features.Employees.Commands.CreateEmployee;
using Application.Features.Employees.Queries.ExportEmployeesBackgroundJob;
using Application.Features.Employees.Queries.GetAllEmployees;
using Application.Features.Employees.Queries.GetEmployeeById;
using Application.Test.Configurations.AutoMoq;
using AutoFixture.Xunit2;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;
using Xunit;

namespace WebApi.Test.Controllers
{
    public class EmployeesControllerTest
    {
        #region GetAsync

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
            var actual = await sut.GetAsync(CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<OkObjectResult>();
            var result = actual.Result as OkObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            result!.Value.Should().Be(employees);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<GetAllEmployeesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region GetEmployeeByIdAsync

        [Theory(DisplayName = "It should return the employee"), AutoMoq]
        public async Task GetEmployeeByIdAsync_Ok(
            GetEmployeeByIdDto employee,
            [Frozen] Mock<IMediator> mockIMediator,
            EmployeesController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<GetEmployeeByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<GetEmployeeByIdDto>.Success(employee));

            //ACT
            var actual = await sut.GetEmployeeByIdAsync(employee.Id, CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<OkObjectResult>();
            var result = actual.Result as OkObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            result!.Value.Should().Be(employee);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<GetEmployeeByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When the employee doesn't exist it returns not found"), AutoMoq]
        public async Task GetEmployeeByIdAsync_NotFound(
            string errorMessage,
            [Frozen] Mock<IMediator> mockIMediator,
            EmployeesController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<GetEmployeeByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<GetEmployeeByIdDto>.Failure(errorMessage));

            //ACT
            var actual = await sut.GetEmployeeByIdAsync(default, CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<NotFoundObjectResult>();
            var result = actual.Result as NotFoundObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result!.Value.Should().Be(errorMessage);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<GetEmployeeByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region CreateAsync

        [Theory(DisplayName = "It should return the new employee Id"), AutoMoq]
        public async Task CreateAsync_Ok(
           int id,
           [Frozen] Mock<IMediator> mockIMediator,
           CreateEmployeeCommand request,
           EmployeesController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<CreateEmployeeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<int>.Success(id));

            //ACT
            var actual = await sut.CreateAsync(request, CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<OkObjectResult>();
            var result = actual.Result as OkObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            result!.Value.Should().Be(id);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<CreateEmployeeCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory(DisplayName = "When there are errors it returns not found"), AutoMoq]
        public async Task CreateAsync_NotFound(
           string errorMessage,
           [Frozen] Mock<IMediator> mockIMediator,
           CreateEmployeeCommand request,
           EmployeesController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<CreateEmployeeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<int>.Failure(errorMessage));

            //ACT
            var actual = await sut.CreateAsync(request, CancellationToken.None);

            //ASSERT
            actual.Result.Should().BeOfType<BadRequestObjectResult>();
            var result = actual.Result as BadRequestObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result!.Value.Should().Be(errorMessage);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<CreateEmployeeCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region ExportFileAsync

        [Theory(DisplayName = "It should return the task id"), AutoMoq]
        public async Task ExportFileAsync_Ok(
           string taskId,
           [Frozen] Mock<IMediator> mockIMediator,
           ExportEmployeesBackgroundJobQuery query,
           EmployeesController sut)
        {
            //ARRANGE
            mockIMediator.Setup(a => a.Send(It.IsAny<ExportEmployeesBackgroundJobQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(taskId);

            //ACT
            var actual = await sut.ExportFileAsync(query, CancellationToken.None);

            //ASSERT
            actual.Should().BeOfType<OkObjectResult>();
            var result = actual as OkObjectResult;
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            result!.Value.Should().Be(taskId);
            mockIMediator.Verify(s => s.Send(
                It.IsAny<ExportEmployeesBackgroundJobQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
    }
}
