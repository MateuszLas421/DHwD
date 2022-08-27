using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Essentials;

namespace DHwD.Service
{
    public interface IGpsService
    {
        Task GetCurrentLocation(TimeSpan timeSpan);
    }
    public class GpsService : IGpsService
    {
        CancellationTokenSource cancellationTokenSource;

        public async Task GetCurrentLocation(TimeSpan timeSpan)
        {
            try
            {
                var request = new GeolocationRequest()
                {
                    DesiredAccuracy = GeolocationAccuracy.Best,
                    Timeout = timeSpan
                };
                cancellationTokenSource = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cancellationTokenSource.Token);

                if (location != null)
                {
                    // We have a location
                }
            }
            catch (FeatureNotSupportedException featureNotSupportedEx)
            {
                // FeatureNotSupportedException
            }
            catch (FeatureNotEnabledException featureNotEnabledEx)
            {
                // FeatureNotEnabledException
            }
            catch (PermissionException permissionEx)
            {
                // PermissionException
            }
            catch (Exception ex)
            {
                // Exception
            }
        }
    }
}
