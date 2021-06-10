namespace SecureOrdering.Core.Dtos.Patient
{
    using SecureOrdering.Core.Dtos.Shared;

    public class PatientParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}