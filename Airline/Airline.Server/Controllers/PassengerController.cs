﻿using Airline.Server.Dto;
using AirLine.Model;
using AirlineModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Airline.Server.Controllers;
/// <summary>
/// Passenger table controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PassengerController : ControllerBase
{
    private readonly IDbContextFactory<AirlineContext> _contextFactory;
    private readonly ILogger<PassengerController> _logger;
    private readonly IMapper _mapper;

    public PassengerController(IDbContextFactory<AirlineContext> contextFactory, ILogger<PassengerController> logger, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get table passengers
    /// </summary>
    /// <returns>
    /// Return all passengers
    /// </returns>
    [HttpGet]
    public async Task<IEnumerable<PassengerGetDto>> Get()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var passenger = await context.Passengers.ToArrayAsync();
        _logger.LogInformation("Get passengers");
        return _mapper.Map<IEnumerable<PassengerGetDto>>(passenger);
    }

    /// <summary>
    /// Get passenger by id
    /// </summary>
    /// <param name="id">Passenger id</param>
    /// <returns>
    /// Return passenger with specified id
    /// </returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PassengerGetDto>> Get(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        _logger.LogInformation($"Get passenger: id ({id})");
        var passenger = await context.FindAsync<Passenger>(id);
        if (passenger == null)
        {
            _logger.LogInformation($"Not found passenger: id ({id})");
            return NotFound();
        }
        else
        {
            _logger.LogInformation($"Get passenger with id {id}");
            return Ok(_mapper.Map<PassengerGetDto>(passenger));
        }
    }


    /// <summary>
    /// Post passenger
    /// </summary>
    /// <param name="passenger"> Passenger class for insert in table</param>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PassengerPostDto passenger)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        _logger.LogInformation("Post passenger");
        await context.Passengers.AddAsync(_mapper.Map<Passenger>(passenger));
        await context.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Put passenger
    /// </summary>
    /// <param name="id">Passenger id for be changed</param>
    /// <param name="passengerToPut">Passenger class for insert in table</param>
    /// <returns>Signalization of success of error</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] PassengerPostDto passengerToPut)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        _logger.LogInformation("Put passenger: id {0}", id);
        var passenger = await context.FindAsync<Passenger>(id);
        if (passenger == null)
        {
            _logger.LogInformation("Not found passenger: id {0}", id);
            return NotFound();
        }
        else
        {
            _logger.LogInformation($"Put passenger with id {id}");
            _mapper.Map(passengerToPut, passenger);
            await context.SaveChangesAsync();
            return Ok();
        }
    }

    /// <summary>
    /// Delete passenger 
    /// </summary>
    /// <param name="id">Passenger id for deleting</param>
    /// <returns>Triggered of success and error</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        _logger.LogInformation($"Put passenger: id ({id})");
        var passenger = await context.FindAsync<Passenger>(id);
        if (passenger == null)
        {
            _logger.LogInformation($"Not found passenger: id ({id})");
            return NotFound();
        }
        else
        {
            context.Passengers.Remove(passenger);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
