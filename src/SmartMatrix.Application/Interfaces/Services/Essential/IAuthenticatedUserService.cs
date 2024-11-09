namespace SmartMatrix.Domain.Interfaces.Services.Essential
{
    public interface IAuthenticatedUserService
    {
        string UserAccountId { get; }
        string UserAccountName { get; }
        string FullName { get; }
    }
}