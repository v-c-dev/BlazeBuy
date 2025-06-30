using Microsoft.AspNetCore.Authorization;

namespace BlazeBuy.Policies
{
    public class OrderOwnerRequirement : IAuthorizationRequirement
    {
    }
}
