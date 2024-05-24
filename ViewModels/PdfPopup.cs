using Mopups.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exportacion.ViewModels
{
    public class PdfPopup : PopupPage
    {
        public PdfPopup(string pdfUrl)
        {
            var webView = new WebView
            {
                Source = pdfUrl,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            Content = new Frame
            {
                Content = webView,
                CornerRadius = 10,
                Padding = 0,
                HasShadow = true,
                BackgroundColor = Colors.White,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 600,
                WidthRequest = 400
            };
        }
    }
}
