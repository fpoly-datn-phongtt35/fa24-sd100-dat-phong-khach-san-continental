namespace Domain.DTO.Room
{
    public class TopBookedRoom
    {
        public Guid RoomId { get; set; }       
        public string RoomName { get; set; }   
        public decimal RoomPrice { get; set; }  
        public int TotalBookings { get; set; }  
        public List<string> RoomImages { get; set; } = new List<string>(); 
    }
}
