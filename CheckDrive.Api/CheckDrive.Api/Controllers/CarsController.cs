﻿using CheckDrive.Application.DTOs.Car;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Mappings.CSV;
using CheckDrive.Application.QueryParameters;
using CheckDrive.Domain.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CheckDrive.Api.Controllers;

[Route("api/cars")]
[ApiController]
public class CarsController(ICarService carService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CarDto>>> GetAllAsync([FromQuery] CarQueryParameters queryParameters)
    {
        var cars = await carService.GetAllAsync(queryParameters);

        return Ok(cars);
    }

    [HttpGet("{id:int}", Name = "GetCarByIdAsync")]
    public async Task<ActionResult<CarDto>> GetCarByIdAsync(int id)
    {
        var car = await carService.GetByIdAsync(id);

        return Ok(car);
    }

    [HttpPost]
    public async Task<ActionResult<CarDto>> CreateAsync([FromBody] CreateCarDto car)
    {
        var createdCar = await carService.CreateAsync(car);

        return CreatedAtAction(nameof(GetCarByIdAsync), new { id = createdCar.Id }, createdCar);
    }

    [HttpPost("upload")]
    public IActionResult Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File not provided or empty");

        if (!file.FileName.EndsWith(".csv"))
            return BadRequest("Invalid file format. Please upload a CSV file.");

        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        });
        csv.Context.RegisterClassMap<CarCsvMappings>();

        try
        {
            var records = csv.GetRecords<Car>().ToList();

            return Ok(new { Message = "CSV processed successfully", RecordCount = records.Count });
        }
        catch (Exception ex)
        {
            return BadRequest($"Error processing CSV: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CarDto>> UpdateAsync([FromRoute] int id, [FromBody] UpdateCarDto car)
    {
        if (id != car.Id)
        {
            return BadRequest($"Route parameter id: {id} does not match with body parameter id: {car.Id}.");
        }

        var updatedCar = await carService.UpdateAsync(car);

        return Ok(updatedCar);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await carService.DeleteAsync(id);

        return NoContent();
    }
}
