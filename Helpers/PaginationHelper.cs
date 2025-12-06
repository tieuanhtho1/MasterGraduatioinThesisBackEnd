namespace WebAPI.Helpers;

public static class PaginationHelper
{
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 100;

    public static (int pageNumber, int pageSize) ValidatePaginationParameters(int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = DefaultPageNumber;
        if (pageSize < 1) pageSize = DefaultPageSize;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;

        return (pageNumber, pageSize);
    }

    public static int CalculateTotalPages(int totalCount, int pageSize)
    {
        return (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public static int CalculateSkip(int pageNumber, int pageSize)
    {
        return (pageNumber - 1) * pageSize;
    }
}
