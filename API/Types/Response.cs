namespace API.Types;

public record Response(
    int error,
    String message,
    object? data
);