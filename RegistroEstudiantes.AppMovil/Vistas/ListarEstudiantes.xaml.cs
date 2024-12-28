using System.Collections.ObjectModel;
using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;

namespace RegistroEstudiantes.AppMovil.Vistas;

public partial class ListarEstudiantes : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://registroestudiantes-5df1f-default-rtdb.firebaseio.com/");
    public ObservableCollection<Estudiante> Lista { get; set; } = new ObservableCollection<Estudiante>();

    public ListarEstudiantes()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarLista(); // Recarga los datos al aparecer la página
    }

    private async void CargarLista()
    {
        Lista.Clear();
        var estudiantes = await client.Child("Estudiantes").OnceAsync<Estudiante>();

        // Filtrar solo los estudiantes activos y evitar duplicados
        var estudiantesActivos = estudiantes.Where(e => e.Object.Estado == true)
                                            .GroupBy(e => e.Key) // Agrupar por ID para evitar duplicados
                                            .Select(g => g.First()) // Tomar solo el primer elemento de cada grupo
                                            .ToList();

        foreach (var estudiante in estudiantesActivos)
        {
            Lista.Add(new Estudiante
            {
                Id = estudiante.Key,
                PrimerNombre = estudiante.Object.PrimerNombre,
                SegundoNombre = estudiante.Object.SegundoNombre,
                PrimerApellido = estudiante.Object.PrimerApellido,
                SegundoApellido = estudiante.Object.SegundoApellido,
                CorreoElectronico = estudiante.Object.CorreoElectronico,
                Edad = estudiante.Object.Edad,
                FechaInicio = estudiante.Object.FechaInicio,
                Estado = estudiante.Object.Estado,
                Curso = estudiante.Object.Curso
            });
        }

        // Establecer la lista filtrada como fuente de datos
        listaCollection.ItemsSource = Lista;
    }

    private void filtroSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = filtroSearchBar.Text.ToLower();
        if (filtro.Length > 0)
        {
            listaCollection.ItemsSource = Lista.Where(x => x.NombreCompleto.ToLower().Contains(filtro));
        }
        else
        {
            listaCollection.ItemsSource = Lista;
        }
    }

    private async void NuevoEstudianteBoton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CrearEstudiante());
    }

    private async void editarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var estudiante = boton?.CommandParameter as Estudiante;

        if (estudiante != null && !string.IsNullOrEmpty(estudiante.Id))
        {
            await Navigation.PushAsync(new EditarEstudiante(estudiante.Id));
        }
        else
        {
            await DisplayAlert("Error", "No se pudo obtener la información del empleado", "OK");
        }
    }

    private async void deshabilitarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var estudiante = boton?.CommandParameter as Estudiante;

        if (estudiante == null)
        {
            await DisplayAlert("Error", "No se pudo obtener la información del empleado", "OK");
            return;
        }

        bool confirmacion = await DisplayAlert("Confirmación", $"Está seguro que desea deshabilitar al empleado {estudiante.NombreCompleto}", "Sí", "No");

        if (confirmacion)
        {
            try
            {
                estudiante.Estado = false;
                await client.Child("Estudiantes").Child(estudiante.Id).PutAsync(estudiante);
                await DisplayAlert("Exito", $"Se ha deshabilitado correctamente al usuario {estudiante.NombreCompleto}", "OK");
                CargarLista();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
