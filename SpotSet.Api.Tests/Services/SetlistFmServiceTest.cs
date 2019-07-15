using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using SpotSet.Api.Models;
using SpotSet.Api.Services;
using SpotSet.Api.Tests.Helpers;
using Xunit;

namespace SpotSet.Api.Tests.Services
{
    public class SetlistFmServiceTest
    {
        [Fact]
        public async void SetlistRequestReturnsASetlistDtoWhenCalledWithSetlistId()
        {
            var testSetlist = "{ \"id\": \"testId\", " +
                              "\"eventDate\": \"30-07-2019\", " +
                              "\"artist\": {\"name\": \"artistName\"}, " +
                              "\"venue\": {\"name\": \"venueName\"}, " +
                              "\"sets\": {\"set\": [{\"song\": [{\"name\": \"songTitle\"}]}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);

            var mockHttpClientFactory = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSetlist);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactory);
            
            var result = await mockSetlistFmService.SetlistRequest("testId");
    
            Assert.IsType<SetlistDto>(result);
            Assert.Equal("testId", result.Id);
            Assert.Equal("07-30-2019", result.EventDate);
            Assert.Equal("artistName", result.Artist.Name);
            Assert.Equal("venueName", result.Venue.Name);
            Assert.Equal("songTitle", result.Sets.Set.First().Song.First().Name);
        }
        
        [Fact]
        public async void SetlistRequestReturnsASetlistDtoWhenCalledWithSetlistIdWhichHasMissingData()
        {
            var testSetlist = "{ \"id\": \"testId\", " +
                              "\"eventDate\": \"30-07-2019\", " +
                              "\"artist\": {\"name\": \"\"}, " +
                              "\"venue\": {\"name\": \"\"}, " +
                              "\"sets\": {\"set\": [{\"song\": [{\"name\": \"\"}]}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);

            var mockHttpClientFactory = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSetlist);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactory);
            
            var result = await mockSetlistFmService.SetlistRequest("testId");
    
            Assert.IsType<SetlistDto>(result);
            Assert.Equal("testId", result.Id);
            Assert.Equal("07-30-2019", result.EventDate);
            Assert.Equal("", result.Artist.Name);
            Assert.Equal("", result.Venue.Name);
            Assert.Equal("", result.Sets.Set.First().Song.First().Name);
        }

        [Fact]
        public async void DatesFromIncomingDataAreProperlyFormattedToMonthDayYear()
        {
            var testSetlist = "{ \"id\": \"testId\", " +
                              "\"eventDate\": \"30-07-2019\", " +
                              "\"artist\": {\"name\": \"artistName\"}, " +
                              "\"venue\": {\"name\": \"venueName\"}, " +
                              "\"sets\": {\"set\": [{\"song\": [{\"name\": \"songTitle\"}]}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);
            
            var mockHttpClientFactory = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSetlist);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactory);
            
            var result = await mockSetlistFmService.SetlistRequest("testId");

            Assert.Equal("07-30-2019", result.EventDate);
        }
        
        [Fact]
        public async void DatesFromIncomingDataAreProperlyFormattedToMonthDayYearWhenDayIsSingleDigit()
        {
            var testSetlist = "{ \"id\": \"testId\", " +
                              "\"eventDate\": \"03-07-2019\", " +
                              "\"artist\": {\"name\": \"artistName\"}, " +
                              "\"venue\": {\"name\": \"venueName\"}, " +
                              "\"sets\": {\"set\": [{\"song\": [{\"name\": \"songTitle\"}]}]}}";
            JObject parsedSetlist = JObject.Parse(testSetlist);
            
            var mockHttpClientFactory = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.OK, parsedSetlist);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactory);
            
            var result = await mockSetlistFmService.SetlistRequest("testId");

            Assert.Equal("07-03-2019", result.EventDate);
        }
        
        [Fact]
        public async void SetlistRequestReturnsNullWhenCalledWithSetlistIdWhichResultsInError()
        {
            var mockHttpClientFactory = TestSetup.CreateMockHttpClientFactory(HttpStatusCode.NotFound);
            var mockSetlistFmService = new SetlistFmService(mockHttpClientFactory);

            var result = await mockSetlistFmService.SetlistRequest("invalidId");

            Assert.Null(result);
        }
    }
}