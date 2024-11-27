using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Application.Interfaces.Services.Essential
{
    public interface ITokenService : IService
    {
        SysToken GenerateToken(SysTokenContent content);
        SysToken GenerateToken(string provider, string loginName, SysUser user);        
    }
}