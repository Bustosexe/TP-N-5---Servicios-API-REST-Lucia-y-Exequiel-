## 🚀 Actualización TP N° 6: Seguridad y Auditoría (Rama `seguridad-frontend`)

¡Hola! En esta rama estuve avanzando con la primera parte del TP6. Estos son los cambios principales en la API:

### 1. Oculté las contraseñas (Secrets Manager)
Saqué la Cadena de Conexión de la Base de Datos y la Clave Secreta del JWT del archivo `appsettings.json`. Los dejé vacíos a propósito para que no se suban a GitHub.
**⚠️ IMPORTANTE PARA CORRER LA API:** Vas a tener que hacer clic derecho en el proyecto en Visual Studio -> *"Administrar secretos del usuario"* (Manage User Secrets) y pegar ahí el JSON con nuestra conexión a la BD y la clave JWT.

### 2. Auditoría con Serilog
Instalé los paquetes NuGet correspondientes y modifiqué el `Program.cs`. Ahora la API guarda automáticamente un archivo de texto diario en la carpeta `Logs/` con el registro de todos los errores y el tiempo de cada petición.

### 3. Conexión con Frontend (CORS)
Dejé configurada la política de CORS (`"PermitirTodo"`) y aseguré el orden estricto de los middlewares en el `Program.cs` (`UseCors` está ANTES que `UseAuthentication`). Esto evita que el navegador bloquee al frontend cuando intentemos conectar los diseños de la app móvil.