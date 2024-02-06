using TaxCalculator.Api.Core.Services;
using TaxCalculator.Api.Data.Repositories;

namespace TaxCalculator.Api.Tests
{
    public class TaxCalculatorServiceUnitTests
    {
        private readonly TaxCalculatorService _sut;

        public TaxCalculatorServiceUnitTests()
        {
            _sut = new TaxCalculatorService(new FeeRepository(), new VehicleRepository());
        }

        [Fact]
        public void Passages_In_Same_Hour_Span_Should_Return_Highest_Fee()
        {
            //arrange
            DateTime[] dateTimeArray = new DateTime[]
             {
                DateTime.Parse("2013-01-03 08:25:00"),
                DateTime.Parse("2013-01-03 08:33:00"),
                DateTime.Parse("2013-01-03 08:39:00"),
             };

            //act
            var result = _sut.GetTax("privatecar", dateTimeArray, "gothenburg");

            //assert
            Assert.Equal(13, result);
        }

        [Fact]
        public void Unsorted_Passages_In_Same_Hour_Span_Should_Return_Highest_Fee()
        {
            //arrange
            DateTime[] dateTimeArray = new DateTime[]
            {
               DateTime.Parse("2013-01-03 08:39:00"),
                DateTime.Parse("2013-01-03 08:25:00"),
                DateTime.Parse("2013-01-03 08:33:00"),
            };

            //act
            var result = _sut.GetTax("privatecar", dateTimeArray, "gothenburg");

            //assert
            Assert.Equal(13, result);
        }

        [Fact]
        public void Passages_Exceeding_60_in_Fee_should_return_60()
        {
            //arrange
            DateTime[] dateTimeArray = new DateTime[]
            {
               DateTime.Parse("2013-01-03 08:39:00"),
                DateTime.Parse("2013-01-03 08:25:00"),
                DateTime.Parse("2013-01-03 08:26:00"),
                DateTime.Parse("2013-01-03 09:22:00"),
                DateTime.Parse("2013-01-03 10:25:00"),
                DateTime.Parse("2013-01-03 11:27:00"),
                DateTime.Parse("2013-01-03 12:43:00"),
                DateTime.Parse("2013-01-03 14:25:00"),
                DateTime.Parse("2013-01-03 18:25:00"),
                DateTime.Parse("2013-01-03 06:25:00"),
                DateTime.Parse("2013-01-03 08:33:00"),
            };

            //act
            var result = _sut.GetTax("privatecar", dateTimeArray, "gothenburg");

            //assert
            Assert.Equal(60, result);
        }

        [Fact]
        public void Passages_In_Different_Hour_Spans_Should_Add_Fees()
        {
            //arrange
            DateTime[] dateTimeArray = new DateTime[]
            {
                DateTime.Parse("2013-01-03 08:25:00"),
                DateTime.Parse("2013-01-03 08:33:00"),
                DateTime.Parse("2013-01-03 09:39:00"),
            };

            //act
            var result = _sut.GetTax("privatecar", dateTimeArray, "gothenburg");

            //assert
            Assert.Equal(21, result);
        }


        [Fact]
        public void Holiday_Should_Return_0_Fee()
        {
            //arrange
            DateTime[] dateTimeArray = new DateTime[]
            {
                DateTime.Parse("2013-01-01 08:25:00"),
                DateTime.Parse("2013-01-01 08:33:00"),
                DateTime.Parse("2013-01-01 09:39:00"),
            };

            //act
            var result = _sut.GetTax("privatecar", dateTimeArray, "gothenburg");

            //assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void July_Should_Return_0_Fee()
        {
            //arrange
            DateTime[] dateTimeArray = new DateTime[]
            {
                DateTime.Parse("2013-07-01 08:25:00"),
            };

            //act
            var result = _sut.GetTax("privatecar", dateTimeArray, "gothenburg");

            //assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Weekend_Should_Not_Return_Fee()
        {
            //arrange
            DateTime[] dateTimeArray = new DateTime[]
            {
                   DateTime.Parse("2013-01-05 08:25:00"),
                DateTime.Parse("2013-01-06 08:33:00"),
                DateTime.Parse("2013-01-06 09:39:00"),
            };

            //act
            var result = _sut.GetTax("privatecar", dateTimeArray, "gothenburg");

            //assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Both_Business_Day_And_Holiday_Should_Return_Fee()
        {
            //arrange
            DateTime[] dateTimeArray = new DateTime[]
            {
                DateTime.Parse("2013-01-01 08:25:00"),
                DateTime.Parse("2013-01-02 08:28:00"),
                DateTime.Parse("2013-01-02 09:39:00"),
            };

            //act
            var result = _sut.GetTax("privatecar", dateTimeArray, "gothenburg");

            //assert
            Assert.Equal(21, result);
        }

        [Theory]
        [InlineData("Diplomat")]
        [InlineData("Emergency")]
        [InlineData("ForeignCar")]
        [InlineData("Military")]
        [InlineData("MotorCycle")]
        public void Vechiles_With_No_Fee_Should_Return_0_Fee(string type)
        {
            //arrange
            DateTime[] dateTimeArray = new DateTime[]
            {
                DateTime.Parse("2013-01-09 08:25:00"),
                DateTime.Parse("2013-01-02 08:28:00"),
                DateTime.Parse("2013-01-02 09:39:00"),
            };

            //act
            var result = _sut.GetTax(type, dateTimeArray, "gothenburg");

            //assert
            Assert.Equal(0, result);
        }


        [Fact]
        public void Scribbled_Dates_Should_Return_89_Fee()
        {
            //arrange
            DateTime[] dateTimeArray = new DateTime[]
            {
                DateTime.Parse("2013-01-14 21:00:00"), // 0 
                DateTime.Parse("2013-01-15 21:00:00"), // 0
                DateTime.Parse("2013-02-07 06:23:27"), // 8
                DateTime.Parse("2013-02-07 15:27:00"), // 13
                DateTime.Parse("2013-02-08 06:27:00"), // 8
                DateTime.Parse("2013-02-08 06:20:27"), // 0
                DateTime.Parse("2013-02-08 14:35:00"), // 8
                DateTime.Parse("2013-02-08 15:29:00"), // 0
                DateTime.Parse("2013-02-08 15:47:00"), // 0 
                DateTime.Parse("2013-02-08 16:01:00"), // 18
                DateTime.Parse("2013-02-08 16:48:00"), // 18
                DateTime.Parse("2013-02-08 17:49:00"), // 13
                DateTime.Parse("2013-02-08 18:29:00"), // 0
                DateTime.Parse("2013-02-08 18:35:00"), // 0
                DateTime.Parse("2013-03-26 14:25:00"), // 8
                DateTime.Parse("2013-03-28 14:07:27") // 0
            };

            //act
            var result = _sut.GetTax("privatecar", dateTimeArray, "gothenburg");

            //assert
            Assert.Equal(89, result);
        }

        [Fact]
        public void Passages_In_Same_Hour_Span_Should_Return_Highest_Fee_2()
        {
            //arrange
            DateTime[] dateTimeArray = new DateTime[]
             {
                DateTime.Parse("2013-01-03 07:00:00"), //18
                DateTime.Parse("2013-01-03 08:00:00"), //13
             };

            //act
            var result = _sut.GetTax("privatecar", dateTimeArray, "gothenburg");

            //assert
            Assert.Equal(31, result);
        }
    }
}