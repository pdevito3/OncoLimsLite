namespace Ordering.WebApi.Features.Patients
{
    using Ordering.Core.Entities;
    using Ordering.Core.Dtos.Patient;
    using Ordering.Core.Exceptions;
    using Ordering.Infrastructure.Contexts;
    using Ordering.Core.Wrappers;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MediatR;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.ApiEndpoints;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System.Text.Json;

    public static class GetPatientList
    {
        public class PatientListQuery : IRequest<PagedList<PatientDto>>
        {
            public PatientParametersDto QueryParameters { get; set; }

            public PatientListQuery(PatientParametersDto queryParameters)
            {
                QueryParameters = queryParameters;
            }
        }

        public class Handler : IRequestHandler<PatientListQuery, PagedList<PatientDto>>
        {
            private readonly OrderingDbContext _db;
            private readonly SieveProcessor _sieveProcessor;
            private readonly IMapper _mapper;

            public Handler(OrderingDbContext db, IMapper mapper, SieveProcessor sieveProcessor)
            {
                _mapper = mapper;
                _db = db;
                _sieveProcessor = sieveProcessor;
            }

            public async Task<PagedList<PatientDto>> Handle(PatientListQuery request, CancellationToken cancellationToken)
            {
                if (request.QueryParameters == null)
                {
                    // log error
                    throw new ApiException("Invalid query parameters.");
                }

                var collection = _db.Patients
                    as IQueryable<Patient>;

                var sieveModel = new SieveModel
                {
                    Sorts = request.QueryParameters.SortOrder ?? "PatientId",
                    Filters = request.QueryParameters.Filters
                };

                var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
                var dtoCollection = appliedCollection
                    .ProjectTo<PatientDto>(_mapper.ConfigurationProvider);

                return await PagedList<PatientDto>.CreateAsync(dtoCollection,
                    request.QueryParameters.PageNumber,
                    request.QueryParameters.PageSize);
            }
        }
    }

    //[Route("/api/patients")]
    //public class GetPatientListEndpoint : BaseAsyncEndpoint
    //    .WithRequest<PatientParametersDto>
    //    .WithResponse<IActionResult>
    //{
    //    private readonly IMediator _mediator;

    //    public GetPatientListEndpoint(IMediator mediator)
    //    {
    //        _mediator = mediator;
    //    }

    //    /// <summary>
    //    /// Gets a list of all Patients.
    //    /// </summary>
    //    /// <response code="200">Patient list returned successfully.</response>
    //    /// <response code="400">Patient has missing/invalid values.</response>
    //    /// <response code="500">There was an error on the server while creating the Patient.</response>
    //    /// <remarks>
    //    /// Requests can be narrowed down with a variety of query string values:
    //    /// ## Query String Parameters
    //    /// - **PageNumber**: An integer value that designates the page of records that should be returned.
    //    /// - **PageSize**: An integer value that designates the number of records returned on the given page that you would like to return. This value is capped by the internal MaxPageSize.
    //    /// - **SortOrder**: A comma delimited ordered list of property names to sort by. Adding a `-` before the name switches to sorting descendingly.
    //    /// - **Filters**: A comma delimited list of fields to filter by formatted as `{Name}{Operator}{Value}` where
    //    ///     - {Name} is the name of a filterable property. You can also have multiple names (for OR logic) by enclosing them in brackets and using a pipe delimiter, eg. `(LikeCount|CommentCount)>10` asks if LikeCount or CommentCount is >10
    //    ///     - {Operator} is one of the Operators below
    //    ///     - {Value} is the value to use for filtering. You can also have multiple values (for OR logic) by using a pipe delimiter, eg.`Title@= new|hot` will return posts with titles that contain the text "new" or "hot"
    //    ///
    //    ///    | Operator | Meaning                       | Operator  | Meaning                                      |
    //    ///    | -------- | ----------------------------- | --------- | -------------------------------------------- |
    //    ///    | `==`     | Equals                        |  `!@=`    | Does not Contains                            |
    //    ///    | `!=`     | Not equals                    |  `!_=`    | Does not Starts with                         |
    //    ///    | `>`      | Greater than                  |  `@=*`    | Case-insensitive string Contains             |
    //    ///    | `&lt;`   | Less than                     |  `_=*`    | Case-insensitive string Starts with          |
    //    ///    | `>=`     | Greater than or equal to      |  `==*`    | Case-insensitive string Equals               |
    //    ///    | `&lt;=`  | Less than or equal to         |  `!=*`    | Case-insensitive string Not equals           |
    //    ///    | `@=`     | Contains                      |  `!@=*`   | Case-insensitive string does not Contains    |
    //    ///    | `_=`     | Starts with                   |  `!_=*`   | Case-insensitive string does not Starts with |
    //    /// </remarks>
    //    [ProducesResponseType(typeof(Response<IEnumerable<PatientDto>>), 200)]
    //    [ProducesResponseType(typeof(Response<>), 400)]
    //    [ProducesResponseType(500)]
    //    [Consumes("application/json")]
    //    [Produces("application/json")]
    //    [SwaggerOperation(
    //        Summary = "Creates a new Patient record.",
    //        OperationId = "GetPatientListEndpoint",
    //        Tags = new[] { "Patients" })
    //    ]
    //    [HttpGet, MapToApiVersion("1.0")]
    //    public override async Task<IActionResult> HandleAsync([FromQuery] PatientParametersDto patientParametersDto,
    //        CancellationToken cancellationToken = new CancellationToken())
    //    {
    //        // add error handling
    //        var query = new GetPatientList.PatientListQuery(patientParametersDto);
    //        var queryResponse = await _mediator.Send(query);

    //        var paginationMetadata = new
    //        {
    //            totalCount = queryResponse.TotalCount,
    //            pageSize = queryResponse.PageSize,
    //            currentPageSize = queryResponse.CurrentPageSize,
    //            currentStartIndex = queryResponse.CurrentStartIndex,
    //            currentEndIndex = queryResponse.CurrentEndIndex,
    //            pageNumber = queryResponse.PageNumber,
    //            totalPages = queryResponse.TotalPages,
    //            hasPrevious = queryResponse.HasPrevious,
    //            hasNext = queryResponse.HasNext
    //        };

    //        Response.Headers.Add("X-Pagination",
    //            JsonSerializer.Serialize(paginationMetadata));

    //        var response = new Response<IEnumerable<PatientDto>>(queryResponse);
    //        return Ok(response);
    //    }
    //}
}