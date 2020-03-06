using DeskBooker.Core.DataRepository;
using DeskBooker.Core.Domain;
using System;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessor
    {
        private IDeskBookingRepository _deskBookingRepository;

        public DeskBookingRequestProcessor(IDeskBookingRepository deskBookingRepository)
        {
            _deskBookingRepository = deskBookingRepository;
        }

        public DeskBookingResult BookDesk(DeskBookingRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(paramName: "request");
            }

            _deskBookingRepository.Save(MapDeskBooking<DeskBooking>(request));
            
            return MapDeskBooking<DeskBookingResult>(request);
        }

        private static T MapDeskBooking<T>(DeskBookingBase request) where T : DeskBookingBase, new()
        {
            return new T
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Date = request.Date
            };
        }
    }
}