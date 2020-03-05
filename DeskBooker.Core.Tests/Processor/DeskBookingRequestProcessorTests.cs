using DeskBooker.Core.Domain;
using System;
using Xunit;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        private DeskBookingRequestProcessor _processor;

        public DeskBookingRequestProcessorTests()
        {
            _processor = new DeskBookingRequestProcessor();

        }

        [Fact]
        public void ShouldReturnBookingResultWithRequestValues()
        {
            //Arrange
            var request = new DeskBookingRequest
            {
                FirstName = "Karthik",
                LastName = "Srivatsa",
                Email = "kms@gmail.com",
                Date = new DateTime(2020, 10, 17)
            };

            //Act
            DeskBookingResult result = _processor.BookDesk(request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.Date, result.Date);
        }

        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            //Arrange processor creation

            //Act/Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));

            //Assert
            Assert.Equal("request", exception.ParamName);

        }
    }
}
