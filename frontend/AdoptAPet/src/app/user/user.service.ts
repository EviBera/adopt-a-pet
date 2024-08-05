import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, Observable, throwError } from 'rxjs';
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

  logout(): Observable<any> {
    return this.http.post("api/auth/logout", {}, {withCredentials: true})
      .pipe(
        map(response => {
          this.user.next(null);
          return response;
        }),
        catchError(this.handleError)
      );
  }

  private handleError(err: any){
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      //client-side or network error 
      errorMessage = `An error occured: ${err.error.message}`;
    } else {
      //backend returned unsuccessful response code
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    console.error(errorMessage);
    return throwError(() => errorMessage);
  }
}
