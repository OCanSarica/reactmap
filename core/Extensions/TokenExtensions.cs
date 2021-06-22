using Microsoft.AspNetCore.Http;
using System;

namespace core.Extensions
{
    public static class TokenExtensions
    {
        public static long GetUserId(this HttpContext _context) =>
            Convert.ToInt64(_context.Request.Headers["token_user_id"]);
    }
}
