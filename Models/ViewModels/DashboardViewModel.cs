namespace DWEB_NET.Models.ViewModels
{
    public class DashboardViewModel
    {
        public decimal AccountValue { get; set; } = 0;
        public List<Transaction> TransactionHistory { get; set; } = new List<Transaction>();
        public List<string> GanhoCategories { get; set; }
        public List<string> GastoCategories { get; set; }
        public string SelectedTransactionType { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal InputValue { get; set; } = decimal.MinValue;
        public Dictionary<string, string> CategoryValues { get; set; }
        public DoughnutData RendasDataGanho { get; set; }
        public DoughnutData RendasDataGasto { get; set; }
    }

    public class Transaction
    {
        public decimal Value { get; set; }
        public string Type { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new List<string>();
        public Dictionary<string, string> CategoryValues { get; set; } = new Dictionary<string, string>();
        public string Descricao { get; set; } = string.Empty;
        public decimal AccountValue { get; set; }
    }

    public class DoughnutData
    {
        public List<string> Labels { get; set; } = new List<string>();
        public List<DoughnutDataset> Datasets { get; set; } = new List<DoughnutDataset>();
    }

    public class DoughnutDataset
    {
        public List<int> Data { get; set; } = new List<int>();
        public List<string> BackgroundColor { get; set; } = new List<string>();
        public List<string> HoverBackgroundColor { get; set; } = new List<string>();
    }
}
