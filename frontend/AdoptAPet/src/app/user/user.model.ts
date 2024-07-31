export interface IUser {
    email: string;
    firstName: string;
    lastName: string;
    userName: string;
    role: string;
    id: string;
    password?: string;
  }
  
  export interface IUserCredentials {
    email: string;
    password: string;
  }