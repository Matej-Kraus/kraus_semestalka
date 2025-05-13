using System;
using System.Collections.Generic;
using System.Linq;
using kraus_semestalka.Data;
using kraus_semestalka.Data.Models;

public static class DataService
{
    public static List<Recordings> GetRecordings()
    {
        using var db = new AppDbContext();
        return db.Recordings.OrderBy(r => r.StartDateTime).ToList();
    }

    public static List<DriveData> GetDriveDataByRecordingId(int recordingId)
    {
        using var db = new AppDbContext();
        return db.DriveData
                 .Where(d => d.RecordingId == recordingId)
                 .OrderBy(d => d.TimeFromStartSeconds)
                 .ToList();
    }
}
