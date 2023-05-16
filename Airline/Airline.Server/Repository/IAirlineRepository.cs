﻿using AirLine.Model;
using AirlineClasses;

namespace Airline.Server.Repository;
public interface IAirlineRepository
{
    List<Flight> Flights { get; }
    List<Passenger> Passengers { get; }
    List<Ticket> Tickets { get; }
    List<Airplane> Airplanes { get; }
    List<FlightAirplaneTicket> FlightAirplaneTickets { get; }


}
