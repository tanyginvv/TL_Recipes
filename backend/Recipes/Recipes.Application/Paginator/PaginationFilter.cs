namespace Recipes.Application.Paginator
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 4;

        public PaginationFilter( int pageNumber, int pageSize )
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
