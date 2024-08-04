using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HealthTracker.Entities.Dtos.Errors;

namespace HealthTracker.Entities.Dtos.Generic;
public class Result<T> //single item return
{
    public T Content { get; set; }

    public Error Error { get; set; }
    public bool isSuccess => Error == null;

    public DateTime ResponseTime { get; set; } = DateTime.UtcNow;



}
