import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ButtonComponent } from '../../building-blocks/button/button.component';
import { ResetPasswordRequest } from '../../interfaces/verify';
import { UserService } from '../../services/user.service';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'app-reset-password',
  imports: [
    ReactiveFormsModule,
    RouterModule,
    CommonModule,
    ButtonComponent
  ],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent {
  resetPasswordForm!: FormGroup;
  token: string | null = null;
  isTokenExpired: boolean = false;
  uniqId: string = "";
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private userService: UserService,
    private snackbar: SnackbarService,
    private router: Router, 
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.token = params.get('id');
      if (!this.token) {
          this.router.navigateByUrl("/login");
      }
      const splitToken = this.token?.split("_");
      if(splitToken)
      {
        this.uniqId = splitToken[0];
        const tokenExpiration = new Date(splitToken[1]);
        if(tokenExpiration < new Date())
        {
          this.isTokenExpired = true;
        }
      }
    });

    this.resetPasswordForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator }); 
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (password?.value !== confirmPassword?.value) {
      return { mismatch: true };
    }
    return null;
  }

  onSubmit(): void {
    if (this.resetPasswordForm.valid && this.token) {
      const newPassword = this.resetPasswordForm.value.password;
      const confirmPassword = this.resetPasswordForm.value.confirmPassword;

      const request = { newPassword: confirmPassword } as ResetPasswordRequest;
      this.userService.resetPassword(this.uniqId, request).subscribe({
        next: (data) => {
          this.snackbar.success("Password reset successfully");
          this.router.navigateByUrl("/login");
        },
        error: (err) => {
          this.snackbar.error("Failed to reset password");
        }
      });
    }
  }
}
