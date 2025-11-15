import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthResponse, LoginRequest, RefreshTokenRequest, RegisterRequest } from '../interfaces/auth-models';
import { environment } from '../../environments/environment.development';
import { ContactRequest } from '../interfaces/contact-request';
import { UserResponse, UserUpdateRequest } from '../interfaces/user';
import { CompleteRegistrationRequest, ForgotPasswordRequest, ResetPasswordRequest } from '../interfaces/verify';
import { ResetPasswordComponent } from '../pages/reset-password/reset-password.component';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  public userInfo = new BehaviorSubject<UserResponse|null>(null);
  userInfo$ = this.userInfo.asObservable();

  
  private userServiceBase: string  = `${environment.gatewayUrl}/${environment.serviceBase.userServiceBase}`
  constructor(private readonly httpClient: HttpClient) { }

  login(model: LoginRequest) : Observable<AuthResponse>
  {
    return this.httpClient.post<AuthResponse>(`${this.userServiceBase}/Auth/Login`, model);
  }

  register(model: RegisterRequest) : Observable<AuthResponse>{
    return this.httpClient.post<AuthResponse>(`${this.userServiceBase}/Auth/Register`, model);
  }

  refreshToken(model: RefreshTokenRequest):Observable<AuthResponse>
  {
    return this.httpClient.post<AuthResponse>(`${this.userServiceBase}/Auth/Refresh`, model);
  }

  getUser():Observable<UserResponse>
  {
    return this.httpClient.get<UserResponse>(`${this.userServiceBase}/User/GetUser`);
  }
  
  updateUser(request: UserUpdateRequest):Observable<UserResponse>
  {
    return this.httpClient.post<UserResponse>(`${this.userServiceBase}/User/Update`, request);
  }

  contact(request: ContactRequest): Observable<void>
  {
    return this.httpClient.post<void>(`${this.userServiceBase}/Notification/Contact`, request);
  }

  completeRegistration(request: CompleteRegistrationRequest): Observable<any>
  {
    return this.httpClient.post<any>(`${this.userServiceBase}/Auth/CompleteRegistration`, request);
  }

  forgetPassword(request: ForgotPasswordRequest): Observable<any>
  {
    return this.httpClient.post<any>(`${this.userServiceBase}/Auth/ForgotPassword`, request, {responseType: 'text' as 'json'});
  }

  resetPassword(id: string, request: ResetPasswordRequest): Observable<any>
  {
    return this.httpClient.post<any>(`${this.userServiceBase}/Auth/ResetPassword/${id}`, request,  {responseType: 'text' as 'json'});
  }
}
