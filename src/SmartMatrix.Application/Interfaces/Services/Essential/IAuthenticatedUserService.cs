namespace SmartMatrix.Application.Interfaces.Services.Essential
{
    public interface IAuthenticatedUserService
    {
        string LoginProviderName { get; }
        string LoginNameIdentifier { get; }
        string UserNameIdentifier { get; }
        string Email { get; }
        string Name { get; }
        string GivenName { get; }
        string Surname { get; }
        List<KeyValuePair<string, string>> Claims { get; set; }
    }
}