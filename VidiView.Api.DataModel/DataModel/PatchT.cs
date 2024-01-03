namespace VidiView.Api.DataModel;

public record Patch<T>(T OldValue, T NewValue);