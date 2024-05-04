using System.Security.Claims;

namespace TP_Redes.Acessos
{
    public static class RoleClaimExtention
    {
        public static IEnumerable<Claim> GetClaims(this Usuarios user)
        {
            var result = new List<Claim>
            {
                new(ClaimTypes.Name, user.nome),
                new(ClaimTypes.Role, user.roleId == 1 ? "comum" : "adm")
            };
            return result;
        }
    }
}
