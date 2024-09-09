namespace View.Models.Paging
{
	public class Paging
	{
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public int TotalPage { get; set; }
		public long TotalRecord { get; set; }
		public string PageAction { get; set; }
		public string RecordName { get; set; }
	}
}
