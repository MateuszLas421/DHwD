using DHwD.Tools;
using Xunit;

namespace Tests.Map
{
    public class DistanceOnMap
    {
        [Theory]
        [InlineData(-12.123365, 23.4540542, -12.2455674, 23.8637924, 46561)]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(30.8617011, 20.5842989, 30.8637924, 20.5914829, 724)]
        public void Distance(double lat1, double lon1, double lat2, double lon2, int fact)
        {
            var value = CalculateCoordinates.DistanceInKmBetweenEarthCoordinates(lat1, lon1, lat2, lon2);
            Assert.Equal(fact, (int)value);
        }
    }
}
