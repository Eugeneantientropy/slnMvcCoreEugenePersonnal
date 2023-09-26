namespace prjMvcCoreEugenePersonnal.Models
{
    [Serializable]
    public class CAlertMessage
    {
        public string AlertStatus { get; set; }
        public string AlertMessage { get; set; }

        public CAlertMessage() { }

        public CAlertMessage(string alertStatus, string alertMessage)
        {
            AlertStatus = alertStatus;
            AlertMessage = alertMessage;
        }
    }
}
