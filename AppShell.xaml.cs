using Exportacion.Views.Seguimiento;

namespace Exportacion
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddUpdateSeguimientoDetail), typeof(AddUpdateSeguimientoDetail));
            Routing.RegisterRoute("seguimientoList", typeof(SeguimientoListPage));
        }
    }
}
