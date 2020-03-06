using DeskBooker.Core.Domain;

namespace DeskBooker.Core.DataRepository
{
    public interface IDeskBookingRepository
    {
        void Save(DeskBooking deskBooking);
    }
}
