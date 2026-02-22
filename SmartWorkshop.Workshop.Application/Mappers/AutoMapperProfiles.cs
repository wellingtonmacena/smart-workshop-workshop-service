using AutoMapper;
using SmartWorkshop.Workshop.Application.UseCases.AvailableServices.Create;
using SmartWorkshop.Workshop.Application.UseCases.People.Create;
using SmartWorkshop.Workshop.Application.UseCases.ServiceOrders.Create;
using SmartWorkshop.Workshop.Application.UseCases.Supplies.Create;
using SmartWorkshop.Workshop.Application.UseCases.Vehicles.Create;
using SmartWorkshop.Workshop.Application.UseCases.Vehicles.Update;
using SmartWorkshop.Workshop.Domain.Common;
using SmartWorkshop.Workshop.Domain.DTOs.AvailableServices;
using SmartWorkshop.Workshop.Domain.DTOs.People;
using SmartWorkshop.Workshop.Domain.DTOs.ServiceOrders;
using SmartWorkshop.Workshop.Domain.DTOs.Supplies;
using SmartWorkshop.Workshop.Domain.DTOs.Vehicles;
using SmartWorkshop.Workshop.Domain.Entities;
using SmartWorkshop.Workshop.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SmartWorkshop.Workshop.Application.Mappers;

[ExcludeFromCodeCoverage]
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {


        // ServiceOrder mappings
        CreateMap<ServiceOrder, ServiceOrderDto>().ReverseMap();
        CreateMap<CreateServiceOrderCommand, ServiceOrder>().ReverseMap();
        CreateMap<Paginate<ServiceOrder>, Paginate<ServiceOrderDto>>().ReverseMap();

        // Quote mappings
        CreateMap<Quote, QuoteDto>().ReverseMap();
        CreateMap<ServiceOrderEvent, ServiceOrderEventDto>().ReverseMap();
    }
}
