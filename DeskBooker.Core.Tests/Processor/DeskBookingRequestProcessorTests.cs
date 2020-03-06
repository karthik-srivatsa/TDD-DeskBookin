using DeskBooker.Core.DataRepository;
using DeskBooker.Core.Domain;
using Moq;
using System;
using Xunit;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        private DeskBookingRequestProcessor _processor;
        private Mock<IDeskBookingRepository> _deskBookingRepositoryMock;
        private DeskBookingRequest _request;

        public DeskBookingRequestProcessorTests()
        {
            _deskBookingRepositoryMock = new Mock<IDeskBookingRepository>();
            _processor = new DeskBookingRequestProcessor(_deskBookingRepositoryMock.Object);
            _request = new DeskBookingRequest
            {
                FirstName = "Karthik",
                LastName = "Srivatsa",
                Email = "kms@gmail.com",
                Date = new DateTime(2020, 10, 17)
            };

        }

        [Fact]
        public void ShouldReturnBookingResultWithRequestValues()
        {
            //Arrange
            //CTOR

            //Act
            DeskBookingResult result = _processor.BookDesk(_request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_request.FirstName, result.FirstName);
            Assert.Equal(_request.LastName, result.LastName);
            Assert.Equal(_request.Email, result.Email);
            Assert.Equal(_request.Date, result.Date);
        }

        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            //Arrange 
            //CTOR

            //Act/Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));

            //Assert
            Assert.Equal("request", exception.ParamName);

        }

        [Fact]
        public void ShouldSaveDeskBooking()
        {
            DeskBooking saveDeskBooking = null;
            _deskBookingRepositoryMock.Setup(x => x.Save(It.IsAny<DeskBooking>()))
                .Callback<DeskBooking>(deskBooking =>
                {
                    saveDeskBooking = deskBooking;
                });

            _processor.BookDesk(_request);

            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Once);

            Assert.NotNull(saveDeskBooking);
            Assert.Equal(_request.FirstName, saveDeskBooking.FirstName);
            Assert.Equal(_request.LastName, saveDeskBooking.LastName);
            Assert.Equal(_request.Email, saveDeskBooking.Email);
            Assert.Equal(_request.Date, saveDeskBooking.Date);
        }
    }
}
