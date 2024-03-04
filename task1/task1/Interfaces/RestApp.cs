using System.Net;
using ThirdParty;
namespace RestApp.Interfaces;

// PROBLEM STATEMENT and DESIRED SOLUTION:
// It's one of MANY classes within the solution that uses ThirdParty.IRestClient,
// but unfortunately some of the API services we call are not very stable and we want to enhance all such usages with retry logic as below:
// 1. By default we want to retry 3 times before failing, but the number should be configurable.
// 2. There should be some timeout between retries(any rule works as for now, but ideally should be extensible).
// 3. If System.Net.WebException is thrown, we want to retry, otherwise - we want to 'fail fast' without retrying.
// 4. If System.Net.WebException is thrown after all attempts, we want to log it and return some 'null' or empty result, otherwise - we want to re-throw the original exception.
// 5. We want to log the exception thrown, but we don't want to log 4 times if we retry.
// 6. Ideally we don't want to change dozens of classes that use IRestClient to apply retry logic.
// 7. Cover all the requirements by unit tests.
// 8. Do not use some built-in or 3rd party solutions for retry policies like Polly

public class RestAppClassThatUsesRestClient : IRestApp
{
    private IRestClient _3rdPartyRestClient;
    private ILogger _logger;
    private AppSettings _settings;

    public RestAppClassThatUsesRestClient(IRestClient restClient, ILogger logger, AppSettings appSettings)
    {
        _3rdPartyRestClient = restClient;
        _logger = logger;
        _settings = appSettings;
    }

    public async Task<TModel> GetSomething<TModel>(string url)
    {
        var retrymiliseconds = _settings.RetryMiliseconds;
        var retryCount = _settings.RetryCount;
        var result = default(TModel);
        for (var retry = 0; retry <= retryCount;)
        {
            try
            {
                result = await _3rdPartyRestClient.Get<TModel>(url);
                break; // Break out of the loop if successful
            }
            catch (WebException ex)
            {
                if (retry == retryCount)
                {
                    _logger.Error(ex);
                    break;
                }
                retry++;
                await Task.Delay(retrymiliseconds);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                break;
            }
        }
        return result;
    }

}