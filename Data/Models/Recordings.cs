using System;
using System.Collections.Generic;

namespace kraus_semestalka.Data.Models;

public partial class Recordings
{
    public int Id { get; set; }

    public DateTime StartDateTime { get; set; }

    public int? DurationMs { get; set; }

    public DateTime? EndDateTime { get; set; }

    public string SensorsDeviceName { get; set; } = null!;

    public DateTime? LastProcessed { get; set; }

    public int Flags { get; set; }

    public int? ParentId { get; set; }

    public int? DriveNumber { get; set; }

    public int? BackupId { get; set; }

    public int LapNumber { get; set; }

    public bool LapSplitted { get; set; }

    public bool Recycled { get; set; }

    public string UIID { get; set; } = null!;

    public virtual ICollection<DriveData> DriveData { get; set; } = new List<DriveData>();

    public virtual ICollection<Recordings> InverseParent { get; set; } = new List<Recordings>();

    public virtual Recordings? Parent { get; set; }
}
