﻿namespace VidiView.Api.DataModel;

public enum AsyncTaskType
{
    VideoTrim,
    VideoTranscoding
}

public enum TaskState
{
    WaitingForExecution,
    Running,
    SuccessfullyCompleted,
    Aborted,
    Failed
}

public record AsyncTaskStatus
{
    public Guid TaskId { get; init; }

    public AsyncTaskType Task { get; init; }

    public IdAndName CreatedBy { get; init; }

    public TaskState State { get; init; } = TaskState.WaitingForExecution;

    public ulong? TotalBytesToProcess { get; init; }

    public ulong? BytesProcessed { get; init; }

    public float? PercentCompleted { get; init; }

    public TimeSpan Elapsed { get; init; }

    public TimeSpan? EstimatedRemainingTime { get; init; }

    public bool NotCompleted => State == TaskState.Running || State == TaskState.WaitingForExecution;

    /// <summary>
    /// Any HAL Rest links associated with this object
    /// </summary>
    [JsonPropertyName("_links")]
    public LinkCollection Links { get; init; }
}