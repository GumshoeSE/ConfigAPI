namespace ConfigAPI.Models
{
    public class LSSConfig : ConfigBase
    {
        public bool MultipleCareCompanies { get; set; }
        public bool SSO { get; set; }
        public bool AdministratorsBankID { get; set; }
        public bool MultipleFiles { get; set; }
        public int NumberOfFilesAllowed { get; set; } = 1;
        public bool CustomRequestHeader { get; set; }
        public string RequestHeaderValue { get; set; } = "X-bitoreq-Auth";
        public bool ExtraField { get; set; }
        public string ExtraFieldName { get; set; } = string.Empty;
        public bool EpostTextPanel { get; set; }
        public bool ClaimIsPaid { get; set; }
        public bool Debit { get; set; }
        public string DebitBaseAddress { get; set; } = "https://support.bitoreq.se/RMS/";
        public string DebitEndpoint { get; set; } = "api/LssClaimsAPI";
        public string DebitCustomer { get; set; } = string.Empty;
    }
}
