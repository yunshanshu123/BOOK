namespace backend.DTOs.Admin
{
    public class BookRankingDto
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int MetricValue { get; set; }
    }
}