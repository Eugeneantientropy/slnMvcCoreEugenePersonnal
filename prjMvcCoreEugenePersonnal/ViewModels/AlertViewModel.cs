using prjMvcCoreEugenePersonnal.Models;

namespace prjMvcCoreEugenePersonnal.ViewModels
{
    public class AlertViewModel
    {
        public CAlertMessage AlertMessage { get; set; }

        public AlertViewModel()
        {
            AlertMessage = new CAlertMessage();
        }
    }
}

