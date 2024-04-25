using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Collections.Generic;

namespace Exportacion
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        string[] ar_codigosR = new string[1000];
        string[] ar_descripcionR = new string[1000];
        int[] ar_cantidadR = new int[1000];
        int[] ar_facturaR = new int[1000];

        int totalRemision = 0;
        int total = 0;
        int total_cajas = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        //private void OnCounterClicked(object sender, EventArgs e)
        //{
        //    count++;

        //    if (count == 1)
        //        CounterBtn.Text = $"Clicked {count} time";
        //    else
        //        CounterBtn.Text = $"Clicked {count} times";

        //    SemanticScreenReader.Announce(CounterBtn.Text);
        //}

        private async void Archivo_Completed(object sender, EventArgs e)
        {
            var entry = sender as Entry;
            if (string.IsNullOrEmpty(entry.Text))
            {
                var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.iOS, new[] { "public.text" } },
            { DevicePlatform.Android, new[] { "text/plain" } },
            { DevicePlatform.WinUI, new[] { ".txt" } }, 
            { DevicePlatform.MacCatalyst, new[] { "public.text" } } 
        });

                var fileResult = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Seleccione un archivo",
                    FileTypes = customFileType
                });

                if (fileResult != null)
                {
                    entry.Text = Path.GetFileNameWithoutExtension(fileResult.FullPath);
                }
                else
                {
                    return;
                }
            }

            if (!string.IsNullOrEmpty(entry.Text))
            {
                ProcessFile(entry.Text);
            }
        }

        private async void ProcessFile(string fileName)
        {
            string baseDir = @"C:\Recamier\Archivos";
            string dirPath = Path.Combine(baseDir, fileName);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                await DisplayAlert("Directorio Creado", $"El directorio {fileName} ha sido creado.", "OK");
            }

        }

        private void OnLimpiarClicked(object sender, EventArgs e)
        {
            archivo.Text = "";
        }

        private void OnSalirClicked(object sender, EventArgs e)
        {
           #if WINDOWS
            Application.Current?.CloseWindow(Application.Current.MainPage.Window);
           #endif

        }

    }

}
