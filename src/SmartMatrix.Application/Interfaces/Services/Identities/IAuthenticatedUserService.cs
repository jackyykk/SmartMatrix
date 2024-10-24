namespace SmartMatrix.Application.Interfaces.Services.Identities
{
    public interface IAuthenticatedUserService
    {
        string UserAccountId { get; }
        string UserAccountName { get; }
        string FullName { get; }
    }
}