using CheckDrive.Application.DTOs.Car;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.QueryParameters;
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
    public async Task<ActionResult<List<CarDto>>> GetAllAsync(CarQueryParameters queryParameters)
    {
        var cars = await _carService.GetAllAsync(queryParameters);

        return Ok(cars);
    }

    [HttpGet("{id:int}", Name = "GetCarByIdAsync")]
    public async Task<ActionResult<CarDto>> GetByIdAsync(int id)
    {
        var car = await _carService.GetByIdAsync(id);

        return Ok(car);
    }

    [HttpPost]
    public async Task<ActionResult<CarDto>> CreateAsync(CreateCarDto car)
    {
        var createdCar = await _carService.CreateAsync(car);

        return CreatedAtAction("GetCarByIdAsync", createdCar, new { id = createdCar.Id });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CarDto>> UpdateAsync(int id, UpdateCarDto car)
    {
        if (id != car.Id)
        {
            return BadRequest($"Route parameter id: {id} does not match with body parameter id: {car.Id}.");
        }

        var updatedCar = await _carService.UpdateAsync(car);

        return Ok(updatedCar);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _carService.DeleteAsync(id);

        return NoContent();
    }
}
