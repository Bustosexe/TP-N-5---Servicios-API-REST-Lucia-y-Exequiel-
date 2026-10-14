using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace GestorComercial_Movil.Handlers
{
    public class TokenAuthenticationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Recuperamos el token guardado durante el Login
            var token = await SecureStorage.Default.GetAsync("jwt_token");

            if (!string.IsNullOrEmpty(token))
            {
                // Inyectamos el token en la cabecera de la petición
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}