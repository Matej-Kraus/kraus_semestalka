using System;
using System.Collections.Generic;

namespace kraus_semestalka.Data.Models;

public partial class DriveData
{
    internal int recordingId;

    public long Id { get; set; }

    public int RecordingId { get; set; }

    public long Time { get; set; }

    public double Lat { get; set; }

    public double Lon { get; set; }

    public float Gx { get; set; }

    public float Gy { get; set; }

    public float Gz { get; set; }

    public float Ax { get; set; }

    public float Ay { get; set; }

    public float Az { get; set; }

    public float Vx { get; set; }

    public float Vy { get; set; }

    public float Vz { get; set; }

    public float Speed { get; set; }

    public float Acceleration { get; set; }

    public float Roll { get; set; }

    public float Pitch { get; set; }

    public float Yaw { get; set; }

    public double? LatRec { get; set; }

    public double? LonRec { get; set; }

    public float? Heading { get; set; }

    public float? HeadingAccuracy { get; set; }

    public float? HorizontaAccuracy { get; set; }

    public float? MslAltitude { get; set; }

    public int? PDoP { get; set; }

    public float? SpeedAccuracyRec { get; set; }

    public float? SpeedRec { get; set; }

    public float? VerticalAccuracy { get; set; }

    public float? WgsAltitude { get; set; }

    public float? CalAx { get; set; }

    public float? CalAy { get; set; }

    public float? CalAz { get; set; }

    public float? CalGx { get; set; }

    public float? CalGy { get; set; }

    public float? CalGz { get; set; }

    public float? TimeFromStartSeconds { get; set; }

    public byte? AxCalibrationStatus { get; set; }

    public byte? AyCalibrationStatus { get; set; }

    public byte? AzCalibrationStatus { get; set; }

    public float? EsfAx { get; set; }

    public float? EsfAy { get; set; }

    public float? EsfAz { get; set; }

    public float? EsfGx { get; set; }

    public float? EsfGy { get; set; }

    public float? EsfGz { get; set; }

    public byte? GxCalibrationStatus { get; set; }

    public byte? GyCalibrationStatus { get; set; }

    public byte? GzCalibrationStatus { get; set; }

    public byte? ImuAuto { get; set; }

    public float? ImuPitch { get; set; }

    public float? ImuRoll { get; set; }

    public byte? ImuStatus { get; set; }

    public float? ImuYaw { get; set; }

    public float? NavPitch { get; set; }

    public float? NavRoll { get; set; }

    public float? NavYaw { get; set; }

    public string? AxCalibrationStatusInfo { get; set; }

    public string? AyCalibrationStatusInfo { get; set; }

    public string? AzCalibrationStatusInfo { get; set; }

    public string? GxCalibrationStatusInfo { get; set; }

    public string? GyCalibrationStatusInfo { get; set; }

    public string? GzCalibrationStatusInfo { get; set; }

    public string? ImuStatusInfo { get; set; }

    public byte? ImuTiltAlgError { get; set; }

    public byte? ImuYawAlgError { get; set; }

    public double? AverageTurnRadius { get; set; }

    public int? CurveTurnDirection { get; set; }

    public double? SegmentCurvature { get; set; }

    public int? SegmentCurvatureLevel { get; set; }

    public double? SegmentRadius { get; set; }

    public double? TurnCurvature { get; set; }

    public int? TurnLevel { get; set; }

    public int? CircuitCurveAccelerationFlag { get; set; }

    public int? CircuitCurveNumber { get; set; }

    public int? CircuitCurveRollFlag { get; set; }

    public string? CircuitCurveTurnDirection { get; set; }

    public bool? InCurve { get; set; }

    public int? CircuitCurveAcceleratingEnd { get; set; }

    public int? CircuitCurveApexClose { get; set; }

    public int? CircuitCurveBrakingStart { get; set; }

    public int? CircuitCurveDirectionBreaks { get; set; }

    public float? CircuitCurveMinApexDistance { get; set; }

    public float? InclPitch { get; set; }

    public float? InclPitchCalOffset { get; set; }

    public float? InclRoll { get; set; }

    public float? InclRollCalOffset { get; set; }

    public float? InclYaw { get; set; }

    public float? InclYawCalOffset { get; set; }

    public int? FixStatus { get; set; }

    public int? CircuitCurveAccelerationBrakingChanges { get; set; }

    public bool? BrakingBeforeTurn { get; set; }

    public bool? BrakingInTurn { get; set; }

    public float? PeakBrakingBeforeTurnDistance { get; set; }

    public int? PeakBrakingBeforeTurnIndex { get; set; }

    public float? PeakBrakingBeforeTurnSpeed { get; set; }

    public float? PeakBrakingInTurnDistance { get; set; }

    public int? PeakBrakingInTurnIndex { get; set; }

    public float? PeakBrakingInTurnSpeed { get; set; }

    public bool? BrakesBeforeInclination { get; set; }

    public float? BrakesBeforeInclinationDistance { get; set; }

    public bool? BrakesDuringInclination { get; set; }

    public float? BrakesBeforeInclinationIndex { get; set; }

    public virtual Recordings Recording { get; set; } = null!;
}
