namespace Ecom.API.Helper
{
    public class Pagination<T> where T : class
    {
        public Pagination(int totalCount, int pageSize, int pageNumber, IReadOnlyList<T> data)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            PageNumber = pageNumber;
            Data = data;
        }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public IReadOnlyList<T> Data { get; set; }
       
    }

}
