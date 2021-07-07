namespace Ordering.WebApi.Extensions.Services
{
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Reflection;
    using Ordering.WebApi.Features.Patients.Services;

    public static class WebApiServiceExtension
    {
        public static void AddWebApiServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));
            services.AddTransient<IPatientLookup, PatientLookup>();

            services.AddMvc()
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}