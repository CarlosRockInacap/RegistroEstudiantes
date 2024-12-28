using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;

namespace RegistroEstudiantes.AppMovil.Vistas;

public partial class CrearEstudiante : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://registroestudiantes-5df1f-default-rtdb.firebaseio.com/");
    public List<Curso> Cursos { get; set; }

    public CrearEstudiante()
    {
        InitializeComponent();
        ListarCursos();
        BindingContext = this;
              
    }

    private void ListarCursos()
    {
        var cursos = client.Child("Cursos").OnceAsync<Curso>();
        Cursos = cursos.Result.Select(x => x.Object).ToList();
    }

    private async void guardarButton_Clicked(object sender, EventArgs e)
    {
        Curso curso = cursoPicker.SelectedItem as Curso;

        var estudiante = new Estudiante
        {
            PrimerNombre = primerNombreEntry.Text,
            SegundoNombre = segundoNombreEntry.Text,
            PrimerApellido = primerApellidoEntry.Text,
            SegundoApellido = segundoApellidoEntry.Text,
            CorreoElectronico = correoEntry.Text,
            FechaInicio = fechaInicioPicker.Date,
            Edad = int.Parse(edadEntry.Text),
            Curso = curso,
            Estado = true
        };

        try
        {
            await client.Child("Estudiantes").PostAsync(estudiante);
            await DisplayAlert("Éxito", $"El Estudiante {estudiante.PrimerNombre} {estudiante.PrimerApellido} fue guardado exitosamente", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error!", $"Ocurrio un error al guardar el empleado: {ex.Message}", "OK");
        }
    }
}