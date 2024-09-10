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
    rememberMe: boolean;
  }

  export interface IUserRegistrationCredentials {
    firstName: string;
    lastname: string;
    email: string;
    password: string;
  }

  export interface IApplicant {
    firstName: string;
    lastName: string;
  }