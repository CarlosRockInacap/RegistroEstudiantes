using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;
using RegistroEstudiantes.Modelos.Modelos;

namespace RegistroEstudiantes.AppMovil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            Registrar();
            return builder.Build();
        }

        public static void Registrar()
        {
            FirebaseClient client = new FirebaseClient("https://registroestudiantes-5df1f-default-rtdb.firebaseio.com/");

            var cursos = client.Child("Cursos").OnceAsync<Curso>();

            if (cursos.Result.Count == 0) 
            {
                client.Child("Cursos").PostAsync(new Curso { Nombre = "1ro Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "2do Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "3ro Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "4to Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "5to Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "6to Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "7mo Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "8vo Basico" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "1ro Medio" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "2do Medio" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "3ro Medio" });
                client.Child("Cursos").PostAsync(new Curso { Nombre = "4to Medio" });
            }
        }
    }
}
