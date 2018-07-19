namespace SenseMining.Importer
{
    public class ConnectionOptions
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string FiscalDataCollectionName { get; set; }

        public override string ToString()
        {
            return
                $"ConnectionString: {ConnectionString}\nDatabaseName: {DatabaseName}\nFiscalDataCollectionName: {FiscalDataCollectionName}";
        }
    }
}
