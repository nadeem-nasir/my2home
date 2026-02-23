using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using model = My2Home.Web.ApiModels;
using entity = My2Home.Core.Entities;



namespace My2Home.Web.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<model.BedViewModel, entity.BedEntity>().ReverseMap();

            CreateMap<model.CityViewModel, entity.CityEntity>().ReverseMap();

            CreateMap < model.CountryViewModel, entity.CountryEntity> ().ReverseMap();

            CreateMap < model.DocumentTypeViewModel, entity.DocumentTypeEntity> ().ReverseMap();

            CreateMap < model.ExpenseCategoryViewModel, entity.ExpenseCategoryEntity> ().ReverseMap();

            CreateMap < model.ExpenseViewModel, entity.ExpenseEntity> ().ReverseMap();

            CreateMap < model.HostelsDocumentViewModel, entity.HostelsDocumentEntity> ().ReverseMap();

            CreateMap < model.HostelViewModel, entity.HostelEntity> ().ReverseMap();

            CreateMap < model.MessSubscriptionViewModel, entity.MessSubscriptionEntity> ().ReverseMap();

            CreateMap < model.OrganizationUserViewModel, entity.OrganizationUserEntity> ().ReverseMap();


            CreateMap < model.OrganizationViewModel, entity.OrganizationEntity> ().ReverseMap();


            

            CreateMap < model.RoomViewModel, entity.RoomEntity> ().ReverseMap();

            CreateMap < model.TenantViewModel, entity.TenantEntity> ().ReverseMap();


            CreateMap<model.RentViewModel, entity.RentEntity>().ReverseMap();

            CreateMap<model.RentListViewModel, entity.RentEntity>().ReverseMap();

            CreateMap<model.RentCreateViewModel, entity.RentEntity>().ReverseMap();

            




        }

    }

}
