using System.Net;

namespace ThirdParty;
public class RestClient : IRestClient
{
    public Task<TModel> Delete<TModel>(int id)
    {
        throw new NotImplementedException();
    }

    public Task<TModel> Get<TModel>(string url)
    {
        // throw new WebException();
        throw new NotImplementedException();
    }

    public Task<TModel> Post<TModel>(string url, TModel model)
    {
        throw new NotImplementedException();
    }

    public Task<TModel> Put<TModel>(string url, TModel model)
    {
        throw new NotImplementedException();
    }
}
