﻿using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.Car;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/cars")]
public class CarsController : Controller
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetCarsAsync(
        [FromQuery] CarResourceParameters carResource)
    {
        var carReviews = await _carService.GetCarsAsync(carResource); ;

        return Ok(carReviews);
    }
    [HttpGet("driverHistories")]
    public async Task<ActionResult<IEnumerable<CarHistoryDto>>> GetCarsHistory([FromQuery] CarResourceParameters carResource)
    {
        var carsHistory = await _carService.GetCarHistories(carResource);

        return Ok(carsHistory);
    }

    [HttpGet("{id}", Name = "GetCarById")]
    public async Task<ActionResult<CarDto>> GetCarByIdAsync(int id)
    {
        var car = await _carService.GetCarByIdAsync(id);

        if (car is null)
            return NotFound($"Car with id: {id} does not exist.");

        return Ok(car);
    }
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] CarForCreateDto forCreateDto)
    {
        var createdCar = await _carService.CreateCarAsync(forCreateDto);

        return CreatedAtAction("GetCarById", new { id = createdCar.Id }, createdCar);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] CarForUpdateDto forUpdateDto)
    {
        if (id != forUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {forUpdateDto.Id}.");
        }

        var updateCar = await _carService.UpdateCarAsync(forUpdateDto);

        return Ok(updateCar);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _carService.DeleteCarAsync(id);

        return NoContent();
    }

    [HttpGet("car/export")]
    public async Task<ActionResult> ExportOperatorToExcel([FromQuery] PropertyForExportFile propertyForExportFile)
    {
        byte[] file = await _carService.MonthlyExcelData(propertyForExportFile);

        if (file == null) return NotFound();

        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Cars.xls");
    }
}
