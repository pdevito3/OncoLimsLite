namespace SecureOrdering.WebApi.Features.Patients.Mappings
{
    using SecureOrdering.Core.Dtos.Patient;
    using AutoMapper;
    using SecureOrdering.Core.Entities;

    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            //createmap<to this, from this>
            CreateMap<Patient, PatientDto>()
                .ReverseMap();
            CreateMap<PatientForCreationDto, Patient>();
            CreateMap<PatientForUpdateDto, Patient>()
                .ReverseMap();
        }
    }
}