using DeskBooker.Core.DataRepository;
using DeskBooker.Core.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        private DeskBookingRequestProcessor _processor;
        private Mock<IDeskBookingRepository> _deskBookingRepositoryMock;
        private Mock<IDeskRepository> _deskRepositoryMock;
        private List<Desk> _availabeDesks;
        private DeskBookingRequest _request;

        public DeskBookingRequestProcessorTests()
        {
            _request = new DeskBookingRequest
            {
                FirstName = "Karthik",
                LastName = "Srivatsa",
                Email = "kms@gmail.com",
                Date = new DateTime(2020, 10, 17)
            };
            _availabeDesks = new List<Desk> { new Desk { Id = 18 } };

            _deskBookingRepositoryMock = new Mock<IDeskBookingRepository>();
            _deskRepositoryMock = new Mock<IDeskRepository>();

            _deskRepositoryMock.Setup(x => x.GetAvailableDesk(_request.Date)).Returns(_availabeDesks);

            _processor = new DeskBookingRequestProcessor(_deskBookingRepositoryMock.Object, _deskRepositoryMock.Object);

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
            Assert.Equal(_availabeDesks.First().Id, saveDeskBooking.DeskId);
        }

        [Fact]
        public void ShouldNotSaveIfNoDeskIsAvailable()
        {
            _availabeDesks.Clear();
            _processor.BookDesk(_request);

            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Never);

        }

        [Theory]
        [InlineData(DeskbookingResultCode.Success, true)]
        [InlineData(DeskbookingResultCode.NotAvailable, false)]
        public void ShouldReturnExpectedResultCode(DeskbookingResultCode expectedResultCode, bool isDeskAvailable)
        {
            if (!isDeskAvailable)
            {
                _availabeDesks.Clear();
            }

            var result = _processor.BookDesk(_request);
            Assert.Equal(expectedResultCode, result.Code);
        }
    }
}
