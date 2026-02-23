using My2Home.Core.Entities;
using My2Home.Web.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My2Home.Web.Helpers
{
    public static class CustomMapper
    {
        public static SelectItemViewModel Map(this CountryEntity countryEntity)
        {
            return new SelectItemViewModel
            {
                Value = countryEntity.CountryId.ToString(),
                Label = countryEntity.CountryName
            };
        }

        public static SelectItemViewModel Map(this HostelEntity hostelEntity)
        {
            return new SelectItemViewModel
            {
                Value = hostelEntity.HostelId.ToString(),
                Label = hostelEntity.HostelName
            };
        }

        public static SelectItemViewModel Map(this ExpenseCategoryEntity expenseCategoryEntity)
        {
            return new SelectItemViewModel
            {
                Value = expenseCategoryEntity.ExpenseCategoryId.ToString(),
                Label = expenseCategoryEntity.ExpenseCategoryName
            };
        }

        public static SelectItemViewModel Map(this RoomEntity roomEntity)
        {
            return new SelectItemViewModel
            {
                Value = roomEntity.RoomId.ToString(),
                Label = roomEntity.RoomNumber.ToString()
            };
        }

        public static SelectItemViewModel Map(this CityEntity cityEntity)
        {
            return new SelectItemViewModel
            {
                Value = cityEntity.CityId.ToString(),
                Label = cityEntity.CityName.ToString()
            };
        }

        public static SelectItemViewModel Map(this TenantEntity tenantEntity)
        {
            return new SelectItemViewModel
            {
                Value = tenantEntity.TenantId.ToString(),
                Label = tenantEntity.TenantName.ToString()
            };
        }
        public static SelectItemViewModel Map(this BedEntity bedEntity)
        {
            return new SelectItemViewModel
            {
                Value = bedEntity.BedNumber.ToString(),
                Label = $"#{bedEntity.BedNumber} of the room {bedEntity.RoomNumber}"
            };
        }

        public static HostelViewModel MapSearch(this HostelEntity hostelEntity)
        {
            return new HostelViewModel
            {
                HostelId = hostelEntity.HostelId,
                HostelName = hostelEntity.HostelName,
                HostelAddress = hostelEntity.HostelAddress,
                HostelContactNumber = hostelEntity.HostelContactNumber,
                HostelContactNumber2 = hostelEntity.HostelContactNumber2,
                HostelContactPersonName = hostelEntity.HostelContactPersonName,
                HostelContactPersonName2 = hostelEntity.HostelContactPersonName2,
                HostelLat = hostelEntity.HostelLat,
                HostelLong = hostelEntity.HostelLong,
                HostelOrganizationId = hostelEntity.HostelOrganizationId,
                HostelCityId = hostelEntity.HostelCityId

            };
        }
    }
}
