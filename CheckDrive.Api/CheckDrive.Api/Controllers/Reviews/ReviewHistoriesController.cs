﻿using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Application.DTOs.DispatcherReview;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Application.DTOs.Review;
using CheckDrive.Application.Interfaces.Review;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Reviews;

[Route("api/reviews/histories")]
[ApiController]
public class ReviewHistoriesController : ControllerBase
{
    private readonly IReviewHistoryService _historyService;

    public ReviewHistoriesController(IReviewHistoryService historyService)
    {
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
    }

    [HttpGet("drivers/{driverId:int}")]
    public async Task<ActionResult<List<CheckPointDto>>> GetDriverHistoriesAsync(int driverId)
    {
        var reviews = await _historyService.GetDriverHistoriesAsync(driverId);

        return Ok(reviews);
    }

    [HttpGet("doctors/{doctorId:int}")]
    public async Task<ActionResult<List<DoctorReviewDto>>> GetDoctorHistoriesAsync(int doctorId)
    {
        var reviews = await _historyService.GetDoctorHistoriesAsync(doctorId);

        return Ok(reviews);
    }

    [HttpGet("mechanics/{mechanicId:int}")]
    public async Task<ActionResult<List<MechanicReviewHistoryDto>>> GetMechanicHistoriesAsync(int mechanicId)
    {
        var reviews = await _historyService.GetMechanicHistoriesAsync(mechanicId);

        return Ok(reviews);
    }

    [HttpGet("operators/{operatorId:int}")]
    public async Task<ActionResult<List<OperatorReviewHistory>>> GetOperatorHistoriesAsync(int operatorId)
    {
        var reviews = await _historyService.GetOperatorHistoriesAsync(operatorId);

        return Ok(reviews);
    }

    [HttpGet("dispatchers/{dispatcherId:int}")]
    public async Task<ActionResult<List<DispatcherReviewHistoryDto>>> GetDispatcherHistoriesAsync(int dispatcherId)
    {
        var reviews = await _historyService.GetDispatcherHistoriesAsync(dispatcherId);

        return Ok(reviews);
    }
}
