namespace TodoWebService.Models.DTOs.Pagination;

public class PaginationMeta
{
    public PaginationMeta(int page, int pageSize, int itemCount)
    {
        Page = page;
        PageSize = pageSize;
        TotalPage = (itemCount + pageSize - 1) / pageSize;
    }

    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPage { get; set; }
}
