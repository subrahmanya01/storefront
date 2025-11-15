import { Component } from '@angular/core';
import { InputComponent } from '../../building-blocks/input/input.component';
import { IconButtonComponent } from '../../building-blocks/icon-button/icon-button.component';
import { ButtonComponent } from '../../building-blocks/button/button.component';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { AuthResponse, RegisterRequest } from '../../interfaces/auth-models';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'app-register',
  imports: [InputComponent, ButtonComponent, IconButtonComponent, ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerForm: any;
  emailOrPhoneRegex = /^(\+?[0-9\s\-().]{7,20}|[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})$/;
  emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
  isSubmitted: boolean = false;
  isAnyErrorWhileRegister: boolean = false;

  isInvalidCredentials: boolean = false;
  constructor(private fb: FormBuilder, 
    private readonly router: Router,
    private readonly snackbar: SnackbarService,
    private readonly authService: AuthService,
    private readonly userService: UserService)
  {
    this.registerForm = this.fb.group({
      name: ['',[Validators.required]],
      emailOrPhone: ['',[Validators.pattern(this.emailOrPhoneRegex), Validators.required]],
      password:['',[Validators.required]]
    })
  }

  onSubmit()
  {
    this.isSubmitted = true;
    if(this.registerForm.valid)
    {
      this.isAnyErrorWhileRegister = false;
      const name = this.registerForm.get("name")?.value;
      const emailOrPhone = this.registerForm.get("emailOrPhone")?.value;
      const password = this.registerForm.get("password")?.value;
      const isEmail = this.emailRegex.test(emailOrPhone);
      const registerRequest = {
        firstName: name,
        lastName: "",
        email: isEmail? emailOrPhone : "",
        phoneNumber: !isEmail? emailOrPhone : "",
        password: password
      } as RegisterRequest;

      this.userService.register(registerRequest).subscribe({
        next: (data: AuthResponse)=>{
          this.router.navigateByUrl("/verify-email");
        },
        error: (err: any)=>{
          this.isAnyErrorWhileRegister = true;
          this.snackbar.error("Failed to register user, Please try again");
          this.isSubmitted = false;
        }
      })
    }
  }
}
