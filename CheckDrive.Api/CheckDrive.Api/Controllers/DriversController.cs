﻿using CheckDrive.ApiContracts.Account;
using CheckDrive.ApiContracts.Driver;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/drivers")]
public class DriversController : Controller
{
    private readonly IDriverService _driverService;
    private readonly IAccountService _accountService;
    public DriversController(IDriverService driverService, IAccountService accountService)
    {
        _driverService = driverService;
        _accountService = accountService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DriverDto>>> GetDriversAsync(
    [FromQuery] DriverResourceParameters driverResource)
    {

        var drivers = await _driverService.GetDriversAsync(driverResource);

        return Ok(drivers);
    }

    [HttpGet("driverHistories")]
    public async Task<ActionResult<IEnumerable<DriverHistoryDto>>> GetDriverHistory(int driverID)
    {
        var driverId = await _driverService.GetDriverIdByAccountId(driverID);

        if (driverId == null)
        {
            return null;
        }

        var historyDrivers = await _driverService.GetDriverHistories(driverId.Id);

        return Ok(historyDrivers);
    }

    [Authorize(Policy = "AdminOrDriver")]
    [HttpGet("{id}", Name = "GetDriverById")]
    public async Task<ActionResult<DriverDto>> GetDriverByIdAsync(int id)
    {
        var driver = await _driverService.GetDriverByIdAsync(id);

        if (driver is null)
            return NotFound($"Driver with id: {id} does not exist.");

        return Ok(driver);
    }

    [Authorize(Policy = "AdminOrDriver")]
    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] DriverForCreateDto driverforCreateDto)
    {
        var createdDriver = await _driverService.CreateDriverAsync(driverforCreateDto);

        return CreatedAtAction("GetDriverById", new { createdDriver.Id }, createdDriver);
    }

    [Authorize(Policy = "AdminOrDriver")]
    [HttpPut("{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] AccountForUpdateDto driverforUpdateDto)
    {
        if (id != driverforUpdateDto.Id)
        {
            return BadRequest(
                $"Route id: {id} does not match with parameter id: {driverforUpdateDto.Id}.");
        }
        var updateDriver = await _accountService.UpdateAccountAsync(driverforUpdateDto);

        return Ok(updateDriver);
    }

    [Authorize(Policy = "AdminOrDriver")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _driverService.DeleteDriverAsync(id);

        return NoContent();
    }
}

