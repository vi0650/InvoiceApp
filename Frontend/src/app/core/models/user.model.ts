export interface User {
  userId: string,
  shopName: string,
  userName: string,
  password: string,
  email: string,
  mobileNo: string,
  address: string,
  role: Role | string | any;
  isActive: boolean;
  createdAt:Date | any;
  updatedAt:Date | any;
}

export enum Role {
  User = 'User',
  Employee = 'Employee',
  SuperAdmin = 'SuperAdmin',
} 