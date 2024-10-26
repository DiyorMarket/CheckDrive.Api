using CheckDrive.Application.DTOs.Car;
using CheckDrive.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/cars")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CarDto>>> GetAvailableCars()
    {
        var cars = await _carService.GetAvailableCarsAsync();

        return Ok(cars);
    }
}
