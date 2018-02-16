
namespace CsvImporter
{
    public class CsvLine
    {
        public string CicId { get; set; }
        public string JrsId { get; set; }
        public string CustomerNumber { get; set; }
        public string HotelName { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return $"Hotel {HotelName}: CicId: {CicId}, JrsId: {JrsId}";
        }
    }
}
