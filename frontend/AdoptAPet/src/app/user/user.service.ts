import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { IUser, IUserCredentials } from '../models/user.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private user: BehaviorSubject<IUser | null>;

  constructor(private http: HttpClient) { 
    this.user = new BehaviorSubject<IUser | null>(null);
  }

  getUser(): Observable<IUser | null>{
    return this.user;
  }

  login(credentials: IUserCredentials): Observable<IUser>{
    return this.http
      .post<IUser>('api/auth/login', credentials)
      .pipe(map((user: IUser) => {
        this.user.next(user);
        console.log(user);
        return user;
      }));
  }
}
