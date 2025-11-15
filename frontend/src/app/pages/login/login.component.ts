import { Component } from '@angular/core';
import { InputComponent } from '../../building-blocks/input/input.component';
import { ButtonComponent } from '../../building-blocks/button/button.component';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AlertsComponent } from '../../building-blocks/alerts/alerts.component';
import { UserService } from '../../services/user.service';
import { AuthResponse, LoginRequest } from '../../interfaces/auth-models';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [
    InputComponent, 
    ButtonComponent, 
    ReactiveFormsModule,
    RouterModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: any;
  isLoginCliked: boolean = false;
  isSubmitted: boolean = false;
  emailOrPhoneRegex = /^(\+?[0-9\s\-().]{7,20}|[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})$/;
  emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

  isInvalidCredentials: boolean = false;
  constructor(private fb: FormBuilder, 
    private readonly router: Router,
    private readonly authService: AuthService,
    private readonly userService: UserService)
  {
    this.loginForm = this.fb.group({
      emailOrPhone: ['',[Validators.pattern(this.emailOrPhoneRegex), Validators.required]],
      password:['',[Validators.required]]
    })
  }

  onSubmit() {
    this.isSubmitted = true;
    if (this.loginForm.valid) {
      this.isLoginCliked = true;
      const emailOrPhone = this.loginForm.get("emailOrPhone").value;
      const password = this.loginForm.get("password").value
      let isEmail = this.emailRegex.test(emailOrPhone);

      let loginRequest = { 
        email: isEmail? emailOrPhone: "", 
        phoneNumber: !isEmail? emailOrPhone: "", 
        password:this.loginForm.get("password").value
      } as LoginRequest;

      this.userService.login(loginRequest).subscribe({
        next: (data: AuthResponse)=>{
          this.authService.setAccessToken(data.token);
          this.authService.setRefreshToken(data.refreshToken);
          this.router.navigateByUrl("/");
        },
        error: (err:any)=>{
          this.isLoginCliked = false;
          this.isInvalidCredentials = true;
        }
      })
    } else {
      console.log('Form Invalid');
    }
  }
}
