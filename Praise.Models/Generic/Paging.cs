namespace Praise.Models.Generic
{
    public class Paging
    {
        // default to page 1
        public int PageNumber { get; set; } = 1;

        // default to 10 per page
        public int NumberPerPage { get; set; } = 10;
    }
}
