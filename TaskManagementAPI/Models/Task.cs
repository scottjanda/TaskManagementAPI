using System;
using System.Collections.Generic;

namespace TaskManagementAPI.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public string? Description { get; set; }

    public string? Details { get; set; }

    public DateTime? Due_Date { get; set; }

    public string? Frequency_Type { get; set; }

    public int? Frequency_Number { get; set; }

    public bool Sensative { get; set; }

    public DateTime? Last_Completed { get; set; }
}
