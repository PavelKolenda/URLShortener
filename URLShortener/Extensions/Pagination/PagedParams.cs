namespace URLShortener.Extensions.Pagination;

public class PagedParams
{
    private int _pageSize;
    public int Page { get; set; }
    public int PageSize { 
        get => _pageSize;
        set
        {
            _pageSize = value switch
            {
                >= 100 => 100,
                <= 0 => 10,
                _ => _pageSize
            };
        }
    }
}
