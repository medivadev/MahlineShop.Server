namespace MahlineShop.Shared.Result;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");
    public static readonly Error InternalServerError = new("Error.InternalServerError", "An unexpected error occurred.");
}
