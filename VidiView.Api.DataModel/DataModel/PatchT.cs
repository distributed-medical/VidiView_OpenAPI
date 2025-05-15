namespace VidiView.Api.DataModel;

[ExcludeFromCodeCoverage]
public record Patch<T>(T OldValue, T NewValue);