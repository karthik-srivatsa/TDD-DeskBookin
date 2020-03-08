using DeskBooker.Core.DataRepository;
using DeskBooker.Core.Domain;
using System;
using System.Linq;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessor
    {
        private IDeskBookingRepository _deskBookingRepository;
        private readonly IDeskRepository _deskRepository;

        public DeskBookingRequestProcessor(IDeskBookingRepository deskBookingRepository, IDeskRepository deskRepository)
        {
            _deskBookingRepository = deskBookingRepository;
            _deskRepository = deskRepository;
        }

        public DeskBookingResult BookDesk(DeskBookingRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(paramName: "request");
            }
            var result = Create<DeskBookingResult>(request);
            var availableDesks = _deskRepository.GetAvailableDesk(request.Date);
            if (availableDesks.FirstOrDefault() is Desk availableDesk)
            {
                var deskBookingObject = Create<DeskBooking>(request);
                deskBookingObject.DeskId = availableDesk.Id;
                _deskBookingRepository.Save(deskBookingObject);
                result.Code = DeskbookingResultCode.Success;
            }
            else
            {
                result.Code = DeskbookingResultCode.NotAvailable;
            }

            return result;
        }

        private static T Create<T>(DeskBookingBase request) where T : DeskBookingBase, new()
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