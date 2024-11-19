using System.Text;
using System.Text.Json;

namespace SmartMatrix.Core.DataSecurity
{
    public class MyHashTool
    {
        private const string UNIVERSAL_SECRET = "SMARTMATRIX!HKG!UNIVERSAL!SECRET!N0PA22W0RD!";
        private const string UNIVERSAL_SALT = "SMARTMATRIX!HKG!SALTYSALT";

        public static string ComputePasswordHash(string loginName, string password)
        {
            StringBuilder sb = new StringBuilder();

            loginName = loginName ?? "";
            password = password ?? "";
            loginName = loginName.Trim();
            password = password.Trim();

            string salt1 = loginName.Length >= 4 ? loginName.Substring(3, loginName.Length - 3) : loginName;
            string salt2 = password.Length >= 4 ? password.Substring(3, password.Length - 3) : password;
            string salt = salt1 + salt2 + UNIVERSAL_SALT;

            sb.Append("<ROOT>");
            sb.Append("<US>");
            sb.Append(JsonSerializer.Serialize(new { UniversalSecret = UNIVERSAL_SECRET }));
            sb.Append("</US>");
            sb.Append("<U>");
                sb.Append(JsonSerializer.Serialize(new { Salt = salt, Password = password }));
            sb.Append("</U>");
            sb.Append("</ROOT>");
            return CryptographyTool.GetSHA512(sb.ToString());
        }        

        public static bool Verify(string loginName, string password, string hash)
        {
            string newHash = ComputePasswordHash(loginName, password);
            return hash == newHash;
        }
    }
}