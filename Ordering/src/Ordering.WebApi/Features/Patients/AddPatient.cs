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
    using Ardalis.ApiEndpoints;
    using Microsoft.AspNetCore.Mvc;
    using Ordering.Core.Wrappers;
    using Swashbuckle.AspNetCore.Annotations;

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
            private readonly IInternalIdGenerator _internalIdGenerator;

            public Handler(OrderingDbContext db, IMapper mapper, IPatientLookup patientLookup, IInternalIdGenerator internalIdGenerator)
            //public Handler(OrderingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _patientLookup = patientLookup;
                _internalIdGenerator = internalIdGenerator;
                _db = db;
            }

            public async Task<PatientDto> Handle(AddPatientCommand request, CancellationToken cancellationToken)
            {
                var patient = new Patient(
                    request.PatientToAdd.FirstName,
                    request.PatientToAdd.LastName,
                    request.PatientToAdd.ExternalId,
                    request.PatientToAdd.Dob,
                    request.PatientToAdd.Gender,
                    request.PatientToAdd.Sex,
                    request.PatientToAdd.Race,
                    request.PatientToAdd.Ethnicity,
                    _internalIdGenerator
                );

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

    [Route("/api/patients")]
    public class PostPatientEndpoint : BaseAsyncEndpoint
        .WithRequest<PatientForCreationDto>
        .WithResponse<Response<PatientDto>>
    {
        private readonly IMediator _mediator;

        public PostPatientEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <response code="201">Patient created.</response>
        /// <response code="400">Patient has missing/invalid values.</response>
        /// <response code="409">A record already exists with this primary key.</response>
        /// <response code="500">There was an error on the server while creating the Patient.</response>
        [ProducesResponseType(typeof(Response<PatientDto>), 201)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(typeof(Response<>), 409)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(
            Summary = "Creates a new Patient record.",
            OperationId = "PostPatientEndpoint",
            Tags = new[] { "Patients" })
        ]
        [HttpPost, MapToApiVersion("1.0")]
        public override async Task<ActionResult<Response<PatientDto>>> HandleAsync([FromBody] PatientForCreationDto patientForCreation,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // add error handling
            var command = new AddPatient.AddPatientCommand(patientForCreation);
            var commandResponse = await _mediator.Send(command);
            var response = new Response<PatientDto>(commandResponse);

            return CreatedAtRoute("GetPatient",
                new { commandResponse.PatientId },
                response);
        }
    }
}