using System;
using System.Collections.Generic;

namespace TaskManagementAPI.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public string? Description { get; set; }

    public string? Details { get; set; }

    public DateTime? DueDate { get; set; }

    public string? FrequencyType { get; set; }

    public int? FrequencyNumber { get; set; }

    public bool? Sensative { get; set; }

    public DateTime? LastCompleted { get; set; }
}
