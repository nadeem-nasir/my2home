import { NbMenuItem } from '@nebular/theme';

export const MENU_ITEMS: NbMenuItem[] = [
  {
    title: 'Dashboard',
    icon: 'nb-home',
    link: '/pages/dashboard',
    home: true,
  },
  {
    title: 'FEATURES',
    group: true,
  },//auth
  //{
  //  title: 'Auth',
  //  icon: '',
  //  children: [
  //    {
  //      title: 'Login',
  //      link: '/auth/login',
  //    },
  //    {
  //      title: 'Register',
  //      link: '/auth/register',
  //    },
  //    {
  //      title: 'Request Password',
  //      link: '/auth/request-password',
  //    },
  //    {
  //      title: 'Reset Password',
  //      link: '/auth/reset-password',
  //    },
  //  ],
  //},
  //hostel
  {
    title: 'Hostel',
    icon: '',
    children: [
      {
        title: 'Hostel list',
        link: '/pages/hostel/hostel-list',
      },

    ],
  },//
  //end hostel
  //room
  {
    title: 'Room',
    icon: '',
    children: [
      {
        title: 'Room list',
        link: '/pages/room/room-list',
      },
      {
        title: 'Generate Room list',
        link: '/pages/room/generate-room-list',
      },
    ],
  },
  //room
  //bed
  {
    title: 'Bed',
    icon: '',
    children: [
      {
        title: 'Bed list',
        link: '/pages/bed/bed-list',
      },
    ],
  },
  //Rent
  {
    title: 'Rent',
    icon: '',
    children: [
      {
        title: 'Monthly rent list',
        link: '/pages/rent/rent-list',
      },
      {
        title: 'Yearly rent list',
        link: '/pages/rent/rent-list-yearly',
      },
      {
        title: 'Generate Rent list',
        link: '/pages/rent/generate-rent-list',
      },
      {
        title: 'Generate Rent single',
        link: '/pages/rent/generate-rent-single',
      },
    ],
  },

  //expense
  {
    title: 'Expense',
    icon: '',
    children: [
      {
        title: 'Expense list',
        link: '/pages/expense/expense-list',
      },
      {
        title: 'Expense create',
        link: '/pages/expense/expense-create-edit',
      },
    ],
  },
  //end expense
  //expenseCategory
  //{
  //  title: 'Expense category',
  //  icon: '',
  //  children: [
  //    {
  //      title: 'Expense category list',
  //      link: '/pages/expenseCategory/expenseCategory-list',
  //    },
  //  ],
  //},
  //expenseCategory
  {//start tenant
    title: 'Tenant',
    icon: '',
    children: [
      {
        title: 'Tenant list',
        link: '/pages/tenant/tenant-list',
      },
      {
        title: 'Tenant create',
        link: '/pages/tenant/tenant-create-edit',
      },

    ],
  },//tenant

  
  //city
  //{
  //  title: 'City',
  //  icon: '',
  //  children: [
  //    {
  //      title: 'City list',
  //      link: '/pages/city/city-list',
  //    },
      
  //  ],
  //},//end city

  //account
  {
    title: 'Account',
    icon: '',
    children: [
      {
        title: 'Account list',
        link: '/pages/account/account-list',
      },
    ],
  },
  //organization
  {
    title: 'Organization',
    icon: '',
    children: [
      {
        title: 'Organization info',
        link: '/pages/organization/organization-list',
      },
    ],
  },
  //end organization
  //Staff
  {
    title: 'Staff',
    icon: '',
    children: [
      {
        title: 'Staff list',
        link: '/pages/staff/staff-list',
      },
      {
        title: 'register staff',
        link: '/pages/staff/register-staff',
      },
    ],
  },
];
