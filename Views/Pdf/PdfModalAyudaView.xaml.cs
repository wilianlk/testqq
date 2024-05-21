namespace Exportacion.Views.Pdf;

public partial class PdfModalAyudaView : ContentView
{
	public PdfModalAyudaView()
	{
		InitializeComponent();
	}
    public void LoadPdf(string pdfFilePath)
    {
        pdfWebView.Source = pdfFilePath;
        pdfWebView.Navigated += (s, e) =>
        {
            pdfWebView.Eval("document.body.style.zoom='150%'");
        };
    }
    private void OnCloseButtonClicked(object sender, EventArgs e)
    {
        this.IsVisible = false;
    }
}