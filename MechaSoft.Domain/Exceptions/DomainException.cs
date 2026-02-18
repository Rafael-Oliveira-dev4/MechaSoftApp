using MechaSoft.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechaSoft.Domain.Exceptions;

public abstract class DomainException : System.Exception
{
    protected DomainException(string message) : base(message) { }
    protected DomainException(string message, System.Exception innerException) : base(message, innerException) { }
}

public class InvalidVehicleLicensePlateException : DomainException
{
    public InvalidVehicleLicensePlateException(string licensePlate)
        : base($"Invalid Portuguese license plate format: {licensePlate}") { }
}

public class InsufficientStockException : DomainException
{
    public InsufficientStockException(string partName, int requested, int available)
        : base($"Insufficient stock for part '{partName}'. Requested: {requested}, Available: {available}") { }
}

public class InvalidServiceOrderStatusException : DomainException
{
    public InvalidServiceOrderStatusException(ServiceOrderStatus currentStatus, ServiceOrderStatus newStatus)
        : base($"Cannot change status from {currentStatus} to {newStatus}") { }
}