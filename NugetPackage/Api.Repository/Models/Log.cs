using System;
using System.Collections.Generic;

namespace Api.Repository.Models;

public partial class Log
{
    public int Id { get; set; }

    public string? Message { get; set; }

    public string? MessageTemplate { get; set; }

    public string? LogLevel { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? Exception { get; set; }

    public string? Properties { get; set; }
}
