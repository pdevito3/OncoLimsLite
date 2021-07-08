namespace Ordering.WebApi.Features.Patients
{
    using Ordering.Core.Entities;
    using Ordering.Core.Dtos.Patient;
    using Ordering.Core.Exceptions;
    using Ordering.Infrastructure.Contexts;
    using Ordering.WebApi.Features.Patients.Validators;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Ordering.Core.Interfaces.Patients;

    public static class AddPatient
    {
        public class AddPatientCommand : IRequest<PatientDto>
        {
            public PatientForCreationDto PatientToAdd { get; set; }

            public AddPatientCommand(PatientForCreationDto patientToAdd)
            {
                PatientToAdd = patientToAdd;
            }
        }

        public class CustomCreatePatientValidation : PatientForManipulationDtoValidator<PatientForCreationDto>
        {
            public CustomCreatePatientValidation()
            {
            }
        }

        public class Handler : IRequestHandler<AddPatientCommand, PatientDto>
        {
            private readonly OrderingDbContext _db;
            private readonly IMapper _mapper;
            private readonly IPatientLookup _patientLookup;

            public Handler(OrderingDbContext db, IMapper mapper, IPatientLookup patientLookup)
            //public Handler(OrderingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _patientLookup = patientLookup;
                _db = db;
            }

            public async Task<PatientDto> Handle(AddPatientCommand request, CancellationToken cancellationToken)
            {
                if (await _patientLookup.PatientIdExists(request.PatientToAdd.PatientId))
                {
                    throw new ConflictException("A patient already exists with this patient id.");
                }

                var patient = _mapper.Map<Patient>(request.PatientToAdd);

                if (await _patientLookup.PatientNameDobExists(patient))
                {
                    throw new ConflictException("A patient already exists with this name and date of birth.");
                }

                _db.Patients.Add(patient);
                var saveSuccessful = await _db.SaveChangesAsync() > 0;

                if (saveSuccessful)
                {
                    return await _db.Patients
                        .ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(p => p.PatientId == patient.PatientId);
                }
                else
                {
                    // add log
                    throw new Exception("Unable to save the new record. Please check the logs for more information.");
                }
            }
        }
    }
}