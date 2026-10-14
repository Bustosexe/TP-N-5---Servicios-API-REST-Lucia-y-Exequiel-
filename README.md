# 📱 Frontend Móvil - Gestor Comercial (TP6)

Esta rama contiene el frontend desarrollado en .NET MAUI con Blazor Hybrid. Para correr la aplicación y ver las pantallas, es necesario configurar un emulador de Android.

## 🚀 Cómo configurar el Emulador en Visual Studio

1. Abre la solución en Visual Studio 2022.
2. En la barra superior, despliega el menú del botón de inicio (donde suele decir "Windows Machine" o el nombre del proyecto) y selecciona **Android Emulators > Android Device Manager** (Administrador de dispositivos Android).
3. Haz clic en el botón **+ Nuevo** (New).
4. Configura el emulador con estos parámetros recomendados:
   * **Nombre:** Pixel 5 - API 33 (o el de tu preferencia)
   * **Dispositivo base:** Pixel 5
   * **SO (Sistema Operativo):** API 33 o API 34 (Android 13/14). *Si te aparece un aviso, haz clic en "Descargar".*
   * **Procesador:** x86_64
5. Haz clic en **Crear** y espera a que termine la configuración.
6. Una vez creado, presiona el botón **Start (Iniciar)** en el administrador para que el celular virtual encienda.
7. Selecciona este nuevo emulador en el botón de Play de Visual Studio y ejecuta el proyecto.

## 🔌 Conexión con la API
Para probar el login y el inventario, la API del TP5 debe estar corriendo en paralelo o ya estar subida al hosting. La URL base de conexión se encuentra configurada en el archivo `MauiProgram.cs`.