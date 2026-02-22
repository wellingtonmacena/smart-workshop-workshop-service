namespace SmartWorkshop.Workshop.Domain.Common;

public class PaginatedRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
