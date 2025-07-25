using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MechaSoft.Application.Common.Responses;

public static class ApplicationError
{
    public static readonly Error NotFound = new("NotFound", "Entity not found");

}

