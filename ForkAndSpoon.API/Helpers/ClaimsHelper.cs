using System.Security.Claims;

namespace ForkAndSpoon.API.Helpers
{
    public static class ClaimsHelper
    {
        public static string GetUserRoleFromClaims(ClaimsPrincipal user)
        {
            // Attemt to retrive the user's role from JWT claims
            var roleClaim = user.FindFirst(ClaimTypes.Role);

            // Return the role if available, otherwise default to "User"
            return roleClaim?.Value ?? "User";
        }

        public static int GetUserIdFromClaims(ClaimsPrincipal user)
        {
            // Attemt to retrive the user ID from JWT claims
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            // If the claim is not found, throw an exception to block unauthorized access
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID claim is missing.");

            // Parse and return the user ID from the claim value
            return int.Parse(userIdClaim.Value);
        }
    }
}