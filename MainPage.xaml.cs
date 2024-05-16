﻿using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using CommunityToolkit.Maui.Views;
using OfficeOpenXml;


namespace Exportacion
{
    public partial class MainPage : ContentPage
    {
        string miUsuario = Environment.UserName;

        private DateTime actual = DateTime.Now;
        private static readonly string formatter = "MM/dd/yyyy";

        string miFecha;
        string miHora;

        string texto;
        string[] ar_codigos = new string[1000];
        string[] ar_descripcion = new string[1000];
        int[] ar_cantidad = new int[1000];

        string[] ar_codigosR = new string[1000];
        string[] ar_descripcionR = new string[1000];
        int[] ar_cantidadR = new int[1000];
        int[] ar_cajasR = new int[1000];
        int[] ar_facturaR = new int[1000];

        int cantReal = 0;
        int cantItem = 0;
        int totalRemision = 0;
        int total = 0;
        int total_cajas = 0;
        int j = 0, jr = 0;
        int cantidad = 0;
        int NroCajas = 0;

        bool sw_grabar = false, sw_copiar = false;

        string carpeta;
        string carpeta_copia = @"C:\Recamier\Archivos\Copia";

        private bool isCapturingPhoto = false;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnUsuario_OnLoaded(object sender, EventArgs e)
        {
            var usuarioLabel = (Label)sender;
            usuarioLabel.Text = Environment.UserName;
        }
        private void OnTime_DateLoaded(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;

            var time_date = sender as Label;
            if (time_date != null)
            {
                time_date.Text = currentDate.ToString("d");
            }
        }
        private void OnTimeLoaded(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;

            var time_date = sender as Label;
            if (time_date != null)
            {
                time_date.Text = currentDate.ToString("T");
            }
        }
        private async void Archivo_Completed(object sender, EventArgs e)
        {
            var entry = sender as Entry;
            carpeta = @"C:\Recamier\Archivos";
            string filePath = Path.Combine(carpeta, $"{entry.Text}.txt");

            if (File.Exists(filePath))
            {
                ProcessFileConfirmation(entry.Text);
            }
            else
            {
                if (string.IsNullOrEmpty(entry.Text))
                {
                    var fileResult = await FilePicker.PickAsync(new PickOptions
                    {
                        PickerTitle = "Seleccione un archivo",
                        FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.text" } },
                    { DevicePlatform.Android, new[] { "text/plain" } },
                    { DevicePlatform.WinUI, new[] { ".txt" } },
                    { DevicePlatform.MacCatalyst, new[] { "public.text" } }
                })
                    });

                    if (fileResult != null)
                    {
                        filePath = fileResult.FullPath;
                        entry.Text = Path.GetFileNameWithoutExtension(filePath);
                        if (File.Exists(filePath))
                        {
                            ProcessFileConfirmation(entry.Text);

                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Archivo no encontrado.", "OK");
                        }
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Archivo no encontrado. Verifique el nombre e intente de nuevo.", "OK");
                }
            }
        }
        private async void ProcessFileConfirmation(string fileName)
        {
            string folderName = fileName;
            string specificFolderPath = Path.Combine(carpeta, folderName);
            string despachoFileName = Path.Combine(specificFolderPath, $"despacho_{fileName}.txt");
            string remisionFileName = Path.Combine(carpeta, $"{fileName}.txt");

            if (!Directory.Exists(specificFolderPath))
            {
                Directory.CreateDirectory(specificFolderPath);
            }

            if (!File.Exists(remisionFileName))
            {
                using (var fs = File.Create(remisionFileName))
                {

                }
            }

            if (!File.Exists(despachoFileName))
            {
                using (var fs = File.Create(despachoFileName))
                {

                }
            }

            fldCodigo.IsEnabled = true;
            fldNroCajas.IsEnabled = true;
            fldCodigo.Focus();

            if (File.Exists(remisionFileName))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(remisionFileName))
                    {
                        string lineaR;
                        int jr = 0;
                        totalRemision = 0;
                        while ((lineaR = sr.ReadLine()) != null)
                        {
                            if (!string.IsNullOrWhiteSpace(lineaR) && lineaR.Length > 68)
                            {
                                Console.WriteLine("cargando remision" + lineaR.Substring(61, 6));
                                int cantidad = int.Parse(lineaR.Substring(61, 6));
                                ar_codigosR[jr] = lineaR.Substring(0, 20).Trim();
                                ar_descripcionR[jr] = lineaR.Substring(21, 39).Trim();
                                ar_cantidadR[jr] = cantidad;
                                ar_facturaR[jr] = int.Parse(lineaR.Substring(68));
                                totalRemision += cantidad;
                                jr++;
                            }
                        }
                        Console.WriteLine($"Total remisión: {totalRemision}");
                        //await Application.Current.MainPage.DisplayAlert("Total remisión", $"Total remisión: {totalRemision}", "OK");
                    }
                }
                catch (Exception er)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Error leyendo remisión: {er.Message}", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "¡No existe Remisión!", "OK");
            }

            texto = null;

            if (File.Exists(despachoFileName))
            {
                Console.WriteLine("Entra a cargar archivo " + Path.GetFileName(despachoFileName) + " " + File.Exists(despachoFileName));
                try
                {
                    using (StreamReader sr = new StreamReader(despachoFileName))
                    {
                        string linea;
                        j = 0;
                        total = 0;
                        total_cajas = 0;
                        while ((linea = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(linea);
                            Console.WriteLine(linea.Substring(17, 3));
                            int cantidad = int.Parse(linea.Substring(27, 5));
                            int NroCajas = int.Parse(linea.Substring(35, 5));
                            total += cantidad;
                            total_cajas += NroCajas;
                            ar_codigosR[j] = linea.Substring(0, 6);
                            ar_cantidadR[j] = cantidad;
                            j++;
                            if (texto == null)
                            {
                                texto = linea;
                            }
                            else
                            {
                                texto += linea;
                            }
                            texto += "\n";
                        }

                    }
                }
                catch (Exception ee)
                {
                    //await Application.Current.MainPage.DisplayAlert("Error", "Error leyendo archivo de despacho: " + ee.Message, "OK");
                }
            }

            double porcentaje = totalRemision > 0 ? ((total * 100.0) / totalRemision) : 0.0;
            string porcentajeFormateado = porcentaje.ToString("N1");

            Console.WriteLine(" %  " + porcentajeFormateado + "%");

            txtCodigos.Text = texto;
            fldCajas.Text = total_cajas.ToString();
            fldUnidades.Text = $"{total} de {totalRemision} ({porcentajeFormateado}%)";

            fldCodigo.Focus();

        }
        private async void OntGuardarAction(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(fldFactura.Text) || fldFactura.Text.Length < 2)
            {
                await DisplayAlert("¡* * * O J O * * *!", "No se ha dado nombre de exportación", "OK");
                fldFactura.Focus();
                return;
            }

            await DisplayAlert("Información", "Favor tome las fotos y al terminar de tomarlas presione Aceptar", "OK");

            bool result = await Fotos("O");
            if (!result)
            {
                await DisplayAlert("Error", "Hubo un problema al tomar las fotos.", "OK");
            }

            fldCodigo.Focus();
        }
        public async Task<bool> Fotos(string tipoFoto)
        {
            if (fldFactura.Text.Length < 2)
            {
                await DisplayAlert("Alerta", "No se ha dado nombre de exportación!", "OK");
                return false;
            }

            string userName = Environment.UserName;

            string nomCarpetaFotos = $@"C:\Users\{userName}\Pictures\Camera Roll";
            string nomCarpetaDestinoFotos = $@"C:\Recamier\Archivos\{fldFactura.Text}";

            if (!Directory.Exists(nomCarpetaFotos))
            {
                Directory.CreateDirectory(nomCarpetaFotos);
            }

            string currentTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            if (tipoFoto == "I")
            {
                if (!Directory.Exists(nomCarpetaDestinoFotos))
                {
                    Directory.CreateDirectory(nomCarpetaDestinoFotos);
                }

                string[] listado_i = Directory.GetFiles(nomCarpetaDestinoFotos);
                for (int a = 0; a < listado_i.Length; a++)
                {
                    string nombreFoto_i = Path.Combine(nomCarpetaDestinoFotos,
                        $"{fldFactura.Text}_{fldCodigo.Text.Substring(0, Math.Min(6, fldCodigo.Text.Length))}_{currentTime.Substring(0, 2)}{currentTime.Substring(2, 2)}{currentTime.Substring(4, 2)}_{a}.jpg");

                    //await CaptureAndHandlePhotoAsync(nombreFoto_i);
                }
            }
            else
            {
                string[] listado = Directory.GetFiles(nomCarpetaFotos);

                if (listado == null || listado.Length == 0)
                {
                    await DisplayAlert("Alerta", "No se encontraron fotos!", "OK");
                    fldCodigo.Focus();
                    return false;
                }
                else
                {
                    for (int i = 0; i < listado.Length; i++)
                    {
                        string archivoOrigen = listado[i];
                        string nombreFoto = "";

                        if (tipoFoto == "O")
                        {
                            nombreFoto = Path.Combine(nomCarpetaDestinoFotos,
                                $"{fldFactura.Text}_{currentTime.Substring(0, 2)}{currentTime.Substring(2, 2)}{currentTime.Substring(4, 2)}_{i}.jpg");

                            try
                            {
                                File.Move(archivoOrigen, nombreFoto, true);
                                Console.WriteLine("OK");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error al mover el archivo: {ex.Message}");
                            }
                        }
                    }
                }
            }

            return true;
        }
        private async void OnfldCodigoCompleted(object sender, EventArgs e)
        {
            string cadena = fldCodigo.Text;
            if (cadena.Length < 19)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Dato ingresado incompleto - intente de nuevo", "OK");
                fldCodigo.Focus();
            }
            else
            {
                Console.WriteLine("Cadena=" + cadena);
                string codigo = cadena.Substring(0, 6);
                string lote = cadena.Substring(6, 6);
                string op = cadena.Substring(12, 6);
                string descripcion = "-";
                descripcion = LeeItem(codigo);
                cantItem = LeeCantFactura(codigo);
                if (cantItem == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "¡Item no existe en esta importación!", "OK");
                    fldCodigo.Text = "";
                    fldNroCajas.Text = "";
                    fldCodigo.Focus();
                    return;
                }
                cantReal = LeeCantReal(codigo);
                Console.WriteLine("op =" + op);
                Console.WriteLine("lote= " + lote);
                fldNroCajas.Text = "0";

                await Application.Current.MainPage.DisplayAlert("Instrucción", "Favor tome las fotos y al terminar de tomarlas presione Aceptar", "OK");
                bool result = await Fotos("I");
                if (result)
                {
                    fldNroCajas.Focus();
                }
            }
        }

        private string LeeItem(string codigo)
        {
            string descripcion = null, cod;
            int i;

            Console.WriteLine("Lee item: " + codigo);

            for (i = 0; i < ar_codigosR.Length; i++)
            {
                if (!string.IsNullOrEmpty(ar_codigosR[i]) && ar_codigosR[i] != null)
                {

                    Console.WriteLine(ar_codigosR[i]);

                    cod = ar_codigosR[i];
                    Console.WriteLine($"i= {i}");
                    Console.WriteLine(ar_codigosR[i] + ar_descripcionR[i]);

                    if (ar_codigosR[i] == null)
                    {
                        break;
                    }

                    Console.WriteLine($"Compara {codigo} Vs {cod} {cod.Trim()} {codigo.Length} {ar_codigosR}");

                    if (cod.Trim().Equals(codigo.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        descripcion = ar_descripcionR[i];
                        Console.WriteLine(descripcion);
                        break;
                    }
                }
            }

            Console.WriteLine("Descripción: " + descripcion);
            return descripcion;
        }
        private int LeeCantFactura(string codigo)
        {
            int cantidadRemision = 0;

            Console.WriteLine("lee cant fact item " + codigo);

            for (int l = 0; l < ar_codigosR.Length; l++)
            {
                string cod = ar_codigosR[l];
                if (cod == null)
                {
                    break;
                }

                if (cod.Trim().Equals(codigo.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    int cant = ar_cantidadR[l];
                    cantidadRemision += cant;
                }
            }

            return cantidadRemision;
        }
        private int LeeCantReal(string codigo)
        {
            int cantidadRemision = 0;

            for (int l = 0; l < ar_codigos.Length; l++)
            {
                string cod = ar_codigos[l];
                if (cod == null)
                {
                    break;
                }

                if (cod.Trim().Equals(codigo.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    int cant = ar_cantidad[l];
                    cantidadRemision += cant;
                }
            }

            Console.WriteLine($"Cantidad despachada {codigo} = {cantidadRemision}");

            return cantidadRemision;
        }
        private async void OnfldNroCajasCompleted(object sender, EventArgs e)
        {

            if (fldNroCajas.Text.Length < 1)
            {
                await DisplayAlert("Atención", "Ojo cantidad nula", "OK");
                fldNroCajas.Text = "0";
                fldNroCajas.Focus();
                return;
            }

            if (!int.TryParse(fldNroCajas.Text, out int wNroCajas) || wNroCajas < 1)
            {
                await DisplayAlert("Atención", "Ojo cantidad en cero o no válida", "OK");
                fldNroCajas.Text = "0";
                fldNroCajas.Focus();
                return;
            }

            Console.WriteLine("Cajas " + fldNroCajas.Text);
            string cadena = fldCodigo.Text;
            Console.WriteLine("Cadena=" + cadena);

            string codigo = cadena.Substring(0, 6);
            string lote = cadena.Substring(6, 6);
            string op = cadena.Substring(12, 6);
            string descripcion = LeeItem(codigo);
            int factura = LeeFactura(codigo);
            FechaHora();

            cantidad = int.Parse(cadena.Substring(18));
            NroCajas = int.Parse(fldNroCajas.Text);
            cantidad *= NroCajas;

            if ((cantidad + cantReal) > cantItem)
            {
                await DisplayAlert("Error", $"Cantidad {cantReal + cantidad} > a cantidad de factura {cantItem}", "OK");
                fldCodigo.Focus();
                return;
            }

            total += cantidad;
            total_cajas += NroCajas;

            NumberFormatInfo nfi = new NumberFormatInfo { NumberDecimalDigits = 1 };
            double porcentaje = (double)(total * 100) / totalRemision;
            Console.WriteLine("  % " + porcentaje);

            fldCajas.Text = total_cajas.ToString();
            fldUnidades.Text = $"{total} de {totalRemision}   ({porcentaje.ToString("N", nfi)}%)";
            Console.WriteLine($"action performed {j} {codigo} {descripcion} lote {lote} op {op} {cantidad} {total}");

            if (descripcion != null)
            {
                string formattedCantidad = cantidad.ToString("D5", CultureInfo.InvariantCulture);
                string formattedNroCajas = NroCajas.ToString("D5", CultureInfo.InvariantCulture);

                if (j == 0)
                {
                    texto = $"{codigo} | {lote} | {op} | {formattedCantidad} | {formattedNroCajas} | {factura} | {descripcion} | {miUsuario} | {miFecha} | {miHora}\n";
                }
                else
                {
                    texto += $"{codigo} | {lote} | {op} | {formattedCantidad} | {formattedNroCajas} | {factura} | {descripcion} | {miUsuario} | {miFecha} | {miHora}\n";
                }

                //botGuardar.IsEnabled = true;
                sw_grabar = true;
                txtCodigos.Text = texto;

                fldCodigo.Text = "";
                j++;
                Grabar();
            }
            else
            {
                await DisplayAlert("Error", "¡Item no válido para este despacho!", "OK");
            }
            fldCodigo.Focus();
        }
        private int LeeFactura(string codigo)
        {
            int l, factura = 0;

            for (l = 0; l < ar_codigosR.Length; l++)
            {
                string cod = ar_codigosR[l];
                if (string.IsNullOrEmpty(cod))
                {
                    break;
                }

                Console.WriteLine($"l= {l}");
                Console.WriteLine($"{ar_codigosR[l]}{ar_descripcionR[l]}");
                Console.WriteLine($"compara {codigo} Vs {cod} {cod.Trim()} {codigo.Length}");

                if (cod.Trim().Equals(codigo.Trim(), StringComparison.InvariantCultureIgnoreCase))
                {
                    factura = ar_facturaR[l];
                    Console.WriteLine(factura);
                    break;
                }
            }

            Console.WriteLine($"Factura: {factura}");
            return factura;
        }
        public void FechaHora()
        {
            actual = DateTime.Now;
            miFecha = actual.ToString(formatter);
            miHora = actual.ToString("h:mm:ss tt", CultureInfo.CurrentCulture);
        }
        private async void OnArqueoActionPerformed(object sender, EventArgs e)
        {
            string cod = "", codR = "";
            int i = 0, n = 0;
            int cantR = 0, cantF = 0;
            int tamanoPantalla = 0;
            bool swArqueo = false;

            string arqueo = "ARQUEO ITEMS DE DESPACHO \n-------------------------------------\n";

            for (i = 0; i < ar_codigosR.Length; i++)
            {
                cantR = 0;
                cantF = ar_cantidadR[i];
                codR = ar_codigosR[i];
                if (codR == null)
                {
                    break;
                }
                for (n = 0; n < ar_codigos.Length; n++)
                {
                    cod = ar_codigos[n];
                    if (cod == null)
                    {
                        break;
                    }

                    if (cod.Trim().Equals(codR.Trim()))
                    {
                        cantR += ar_cantidad[n];
                        Console.WriteLine($"i={i}  n={n}  {ar_codigosR[i]} - {ar_codigos[n]} Cant R {ar_cantidadR[i]}  Cant {ar_cantidad[n]}");
                    }
                }

                tamanoPantalla++;
                if (tamanoPantalla > 15)
                {
                    tamanoPantalla = 0;
                    arqueo += "...";
                    await DisplayAlert("Arqueo", arqueo, "OK");
                    arqueo = "ITEMS CON PENDIENTES DE DESPACHO \n-------------------------------------\n...\n";
                }
                swArqueo = true;
                arqueo += $"{ar_codigosR[i]} {ar_descripcionR[i]}              {cantR} de {ar_cantidadR[i]}\n";
            }

            if (swArqueo)
            {
                await DisplayAlert("Arqueo", arqueo, "OK");
            }
            else
            {
                arqueo += "\n\n TODO OK";
                await DisplayAlert("Arqueo", arqueo, "OK");
            }

            fldCodigo.Focus();
        }
        private void OnLimpiarClicked(object sender, EventArgs e)
        {
            if (sw_grabar)
            {
                Grabar();
            }

            texto = null;
            j = 0;
            cantidad = 0;
            total = 0;
            totalRemision = 0;

            fldFactura.Text = "";
            txtCodigos.Text = "";
            fldCodigo.Text = "";
            fldCajas.Text = "";
            fldUnidades.Text = "";
            fldNroCajas.Text = "";

            //botGuardar.IsEnabled = true;
            fldCodigo.IsReadOnly = true;
            fldFactura.Focus();
        }
        public async void Grabar()
        {
            bool swError = false;
            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            string nombreArchivo = Path.Combine(carpeta, fldFactura.Text, $"despacho_{fldFactura.Text}.txt");
            string nombreArchivoExcel = Path.Combine(carpeta, fldFactura.Text, $"despacho_{fldFactura.Text}.xlsx");

            try
            {
                using (StreamWriter writer = new StreamWriter(nombreArchivo, false))
                {
                    writer.WriteLine(texto);
                    Console.WriteLine($"Archivo guardado exitosamente en {nombreArchivo}");
                }

                string[] lineas = File.ReadAllLines(nombreArchivo);
                // Establecer el contexto de la licencia de EPPlus antes de crear una instancia de ExcelPackage

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja = excel.Workbook.Worksheets.Add("Despacho");

                    for (int i = 0; i < lineas.Length; i++)
                    {
                        var columnas = lineas[i].Split('|');

                        for (int j = 0; j < columnas.Length; j++)
                        {
                            hoja.Cells[i + 1, j + 1].Value = columnas[j];
                        }
                    }

                    FileInfo excelFile = new FileInfo(nombreArchivoExcel);
                    excel.SaveAs(excelFile);
                    Console.WriteLine($"Archivo de Excel guardado exitosamente en {nombreArchivoExcel}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error al escribir el archivo:");
                Console.WriteLine(ex.Message);
                swError = true;
            }

            sw_grabar = false;
            sw_copiar = true;

        }
        private void OnSalirClicked(object sender, EventArgs e)
        {
#if WINDOWS
            Application.Current?.CloseWindow(Application.Current.MainPage.Window);
#endif
        }
        private async Task CaptureAndHandlePhotoAsync(string filePath)
        {
            try
            {
                // Verificar si la cámara está disponible
                if (!MediaPicker.Default.IsCaptureSupported)
                {
                    Console.WriteLine("La captura de fotos no es soportada en este dispositivo.");
                    return;
                }

                // Capturar la foto
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo == null)
                {
                    Console.WriteLine("No se capturó ninguna foto.");
                    return;
                }

                // Crear el directorio si no existe
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Guardar la foto en la ruta especificada
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.Create(filePath))
                {
                    await stream.CopyToAsync(newStream);
                }

                Console.WriteLine($"La foto fue procesada y guardada en: {filePath}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error de archivo: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
            }
        }
        private async void OnFormularioDeSeguimientoClicked(object sender, EventArgs e)
            {
                await Shell.Current.GoToAsync("seguimientoList");
            }
        private async void OnBuscarFileInspeccion(object sender, EventArgs e)
            {
                string searchDirectory = @"C:\Recamier\Archivos\";
                string fileName = "regis_personas.xlsx";
                string sourcePath = Path.Combine(searchDirectory, fileName);

                if (!File.Exists(sourcePath))
                {
                    Console.WriteLine("No se encontró el archivo regis_personas.xlsx.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(fldFactura.Text))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Seleccione un archivo Exportacion", "OK");
                    return;
                }

                string folderName = Path.Combine(searchDirectory, fldFactura.Text);

                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }

                string newFileName = $"{fldFactura.Text}_Inspeccion.xlsx";
                string destinationPath = Path.Combine(folderName, newFileName);

                try
                {
                    if (File.Exists(destinationPath))
                    {
                        await Launcher.OpenAsync(new OpenFileRequest
                        {
                            File = new ReadOnlyFile(destinationPath)
                        });
                    }
                    else
                    {
                        File.Copy(sourcePath, destinationPath, overwrite: true);

                        await Launcher.OpenAsync(new OpenFileRequest
                        {
                            File = new ReadOnlyFile(destinationPath)
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al copiar o abrir el archivo: {ex.Message}");
                }
            }
        

    }

}
