﻿namespace Airline.Server.Dto;

/// <summary>
/// Class for post passenger
/// </summary>
public class PassengerPostDto
{
    /// <summary>
    /// Represent a passport number 
    /// </summary>
    public string? PassportNumber { get; set; }
    /// <summary>
    /// Represent a passenger name
    /// </summary>
    public string? Name { get; set; }
}