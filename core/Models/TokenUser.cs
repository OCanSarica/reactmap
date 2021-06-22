using System.Collections.Generic;

namespace core.Models
{
    public class TokenUser
    {
        public List<string> Roles { get; set; }

        public List<string> Permissions { get; set; }

        public long UserId { get; set; }

        public string Email { get; set; }
    }

    public sealed class TokenValidation
    {
        public bool Validated => _Errors.Count == 0;

        public readonly List<TokenValidationStatus> _Errors =
            new List<TokenValidationStatus>();

        public TokenUser TokenUser { get; set; }
    }

    public enum TokenValidationStatus
    {
        Expired,
        WrongUser,
        WrongKey,
        WrongGuid,
        Unexpected
    }
}
