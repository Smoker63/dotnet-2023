﻿using Airline.Server.Dto;
using AirLine.Model;
using AirlineClasses;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Airlines.Server.Controllers;

/// <summary>
/// Airplane controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AirplaneController : ControllerBase
{
    private readonly IDbContextFactory<AirlineContext> _contextFactory;
    private readonly ILogger<AirplaneController> _logger;
    private readonly IMapper _mapper;

    public AirplaneController(IDbContextFactory<AirlineContext> contextFactory, ILogger<AirplaneController> logger, IMapper mapper)
    {
        _contextFactory = contextFactory;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Get airplane table
    /// </summary>
    /// <returns>
    /// Return all airplanes
    /// </returns>
    [HttpGet]
    public async Task<IEnumerable<AirplaneGetDto>> Get()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        _logger.LogInformation("Get airplanes");
        return _mapper.Map<IEnumerable<AirplaneGetDto>>(context.Airplanes);
    }


    /// <summary>
    /// Get airplane by id
    /// </summary>
    /// <returns>
    /// Return airplane with specified id
    /// </returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<AirplaneGetDto>> Get(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        _logger.LogInformation($"Get airplane: id ({id})");
        var airplane = context.Airplanes.FirstOrDefault(airplane => airplane.Id == id);
        if (airplane == null)
        {
            _logger.LogInformation($"Not found airplane: id ({id})");
            return NotFound();
        }
        else
        {
            return Ok(_mapper.Map<AirplaneGetDto>(airplane));
        }
    }

    /// <summary>
    /// Post airplane
    /// </summary>
    /// <param name="airplane"> Airplane class for insert in table</param>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AirplanePostDto airplane)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        _logger.LogInformation("Post airplane");
        context.Airplanes.Add(_mapper.Map<Airplane>(airplane));
        context.SaveChanges();
        return Ok();
    }

    /// <summary>
    /// Put airplane
    /// </summary>
    /// <param name="id">Airplane id for be changed</param>
    /// <param name="airplaneToPut">Airplane class for insert in table</param>
    /// <returns>Triggered of success and error</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] AirplanePostDto airplaneToPut)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        _logger.LogInformation("Put airplane: id {0}", id);
        var airplane = context.Airplanes.FirstOrDefault(airplane => airplane.Id == id);
        if (airplane == null)
        {
            _logger.LogInformation("Not found airplane: id {0}", id);
            return NotFound();
        }
        else
        {
            _mapper.Map(airplaneToPut, airplane);
            context.SaveChanges();
            return Ok();
        }
    }

    /// <summary>
    /// Delete airplane 
    /// </summary>
    /// <param name="id">Airplane id for deleting</param>
    /// <returns>Triggered of success and error</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        _logger.LogInformation($"Put airplane: id ({id})");
        var airplane = context.Airplanes.FirstOrDefault(airplane => airplane.Id == id);
        if (airplane == null)
        {
            _logger.LogInformation($"Not found airplane: id ({id})");
            return NotFound();
        }
        else
        {
            context.Airplanes.Remove(airplane);
            context.SaveChanges();
            return Ok();
        }
    }
}