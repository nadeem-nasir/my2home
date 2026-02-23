using System;
using System.Collections.Generic;
using System.Text;

namespace My2Home.Core.CommonEnums
{
    public enum HostelType
    {
        Male,
        Femail
    }
    public enum UsersTypes
    {
        admin, 
        staff
    }
    public enum UserChannel
    {
        WebSite,
        Andriod,
        Iphone
    }

    public enum UserRoles
    {
        superadmin,
        organizationadmin,
        hosteladmin,
        tenant,
        Marketing
    }

    public enum NotificationsTypes
    {
        MobileNotification,
        SMS,
        Email
    }

    public enum PropertyStatus
    {
        Occupied,
        UnOccupied,
        Maintenance

    }

    public enum RoomType
    {
        Private,
        Dorm
    }

    public enum TenantStatus
    {
        active,
        inactive
    }

    public enum RentStatus
    {
        due,
        paid
    }
}
