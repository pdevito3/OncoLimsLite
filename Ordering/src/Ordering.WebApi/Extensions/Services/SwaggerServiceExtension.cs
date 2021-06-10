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

    public static class SwaggerServiceExtension
    {
        public static void AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "OncoLimsLite Ordering",
                        Description = "This is my swagger doc for ordering",
                        Contact = new OpenApiContact
                        {
                            Name = "Paul",
                            Email = "paul@oncolimslite.com",
                            Url = new Uri("https://www.oncolimslite.com"),
                        },
                    });

                config.IncludeXmlComments(string.Format(@$"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}Ordering.WebApi.xml"));
            });
        }
    }
}