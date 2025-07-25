using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MechaSoft.Application.Common.Exceptions;
public abstract class ApplicationException : Exception
{
    //public ApplicationException() { }

    public ApplicationException(string msg) : base(msg) { }
}
