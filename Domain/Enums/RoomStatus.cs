namespace Domain.Enums
{
    public enum RoomStatus
    {
        Vacant = 1,//trống
        OutOfOrder = 2,
        Deleted = 3,//Bị xóa
        Occupied = 4,//Không trống
        Reserved = 5,
        Cleaned = 6,
        Dirty = 7,
        Inspected = 8,
        DoNotDisturb = 9,
        CheckIn = 10,
        CheckOut = 11,
        AwaitingConfirmation = 12
    }
}
