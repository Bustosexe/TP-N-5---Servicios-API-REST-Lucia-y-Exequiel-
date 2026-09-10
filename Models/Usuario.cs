using Microsoft.AspNetCore.Authorization;

namespace GestorComercial_API.Models
{
    [Authorize]
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }

        // Aquí guardaremos la contraseña encriptada por seguridad
        public string PasswordHash { get; set; }

        // Permite definir permisos, por ejemplo: "Admin", "Vendedor"
        public string Rol { get; set; }
    }
}
