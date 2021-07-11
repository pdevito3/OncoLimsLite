namespace Ordering.WebApi.Features.Patients
{
    using Ordering.Core.Dtos.Patient;
    using Ordering.Core.Exceptions;
    using Ordering.Infrastructure.Contexts;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Ardalis.ApiEndpoints;
    using Microsoft.AspNetCore.Mvc;
    using Ordering.Core.Wrappers;
    using Swashbuckle.AspNetCore.Annotations;

    public static class GetPatient
    {
        public class PatientQuery : IRequest<PatientDto>
        {
            public Guid PatientId { get; set; }

            public PatientQuery(Guid patientId)
            {
                PatientId = patientId;
            }
        }

        public class Handler : IRequestHandler<PatientQuery, PatientDto>
        {
            private readonly OrderingDbContext _db;
            private readonly IMapper _mapper;

            public Handler(OrderingDbContext db, IMapper mapper)
            {
                _mapper = mapper;
                _db = db;
            }

            public async Task<PatientDto> Handle(PatientQuery request, CancellationToken cancellationToken)
            {
                // add logger (and a try catch with logger so i can cap the unexpected info)........ unless this happens in my logger decorator that i am going to add?

                var result = await _db.Patients
                    .ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(p => p.PatientId == request.PatientId);

                if (result == null)
                {
                    // log error
                    throw new KeyNotFoundException();
                }

                return result;
            }
        }
    }

    [Route("/api/patients")]
    public class GetPatientEndpoint : BaseAsyncEndpoint
        .WithRequest<Guid>
        .WithResponse<PatientDto>
    {
        private readonly IMediator _mediator;

        public GetPatientEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a single Patient by ID.
        /// </summary>
        /// <response code="200">Patient record returned successfully.</response>
        /// <response code="400">Patient has missing/invalid values.</response>
        /// <response code="500">There was an error on the server while creating the Patient.</response>
        [ProducesResponseType(typeof(Response<PatientDto>), 200)]
        [ProducesResponseType(typeof(Response<>), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{patientId}", Name = "GetPatient"), MapToApiVersion("1.0")]
        [SwaggerOperation(
            Summary = "Creates a new Patient record.",
            OperationId = "GetPatientEndpoint",
            Tags = new[] { "Patients" })
        ]
        public override async Task<ActionResult<PatientDto>> HandleAsync([FromRoute] Guid patientId,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // add error handling
            var query = new GetPatient.PatientQuery(patientId);
            var queryResponse = await _mediator.Send(query);

            var response = new Response<PatientDto>(queryResponse);
            return Ok(response);
        }
    }
}