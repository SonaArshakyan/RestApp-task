namespace RestApp.Interfaces;
public interface IRestApp
{
    public Task<TModel> GetSomething<TModel>(string url);
}
