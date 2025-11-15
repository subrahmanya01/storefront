import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonComponent } from '../../building-blocks/button/button.component';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { ForgotPasswordRequest } from '../../interfaces/verify';

@Component({
  selector: 'app-forgot-password',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ButtonComponent,
    RouterModule,
  ],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {
  forgotPasswordForm!: FormGroup;
  showEmailSentMessage: boolean = false;
  constructor(
    private fb: FormBuilder,
    private readonly router: Router,
    private readonly userService: UserService
  ) {}

  ngOnInit(): void {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit(): void {
    if (this.forgotPasswordForm.valid) {
      const email = this.forgotPasswordForm.value.email;
      const request =  {email: email} as ForgotPasswordRequest;
      this.userService.forgetPassword(request).subscribe({
        next: (data) => {
          this.showEmailSentMessage = true;
        },
        error: (err) => {
          console.error(err.message);
        }
      });
    }
  }
}
