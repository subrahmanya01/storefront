import { Component, OnInit } from '@angular/core';
import { BreadScrumComponent } from '../../building-blocks/bread-scrum/bread-scrum.component';
import { ButtonComponent } from '../../building-blocks/button/button.component';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { UserResponse } from '../../interfaces/user';
import { UserService } from '../../services/user.service';
import { AuthService } from '../../services/auth.service';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'app-user-profile',
  imports: [
     BreadScrumComponent,
     ButtonComponent,
     ReactiveFormsModule
    ],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit {
  userForm: any;
  userInfo: UserResponse = {} as UserResponse;

  constructor(private fb: FormBuilder, private readonly userService: UserService, 
    private readonly snackbarService: SnackbarService,
    private readonly authService: AuthService)
  {
    this.userForm = this.fb.group({
      firstName: ["", Validators.required],
      lastName: [""],
      email:["", [Validators.required, Validators.email]],
      phoneNumber: ["", [
        Validators.pattern(/^\+?[0-9\s\-]{10,20}$/),
        Validators.minLength(10)
      ]],
      currentPassword: [" "],
      newPassword: [""],
      confirmPassword: [""]
    }, { validator: [this.mustMatch('newPassword', 'confirmPassword'), this.passwordFieldsValidation()] })
  }

  ngOnInit(): void {
    this.userService.userInfo$.subscribe({
      next: (data: UserResponse |null)=>{
        if(!data) return;
        this.userInfo = data;
        this.patchForm();
      }
    })
  }

  patchForm()
  {
    this.userForm?.patchValue({
      firstName: this.userInfo.firstName,
      lastName: this.userInfo.lastName,
      email: this.userInfo.email,
      phoneNumber: this.userInfo.phoneNumber,
      currentPassword: ""
    });
  }
  
  passwordFieldsValidation(): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
      const currentPassword = group.get('currentPassword');
      const newPassword = group.get('newPassword');
      const confirmPassword = group.get('confirmPassword');
  
      if ((currentPassword && currentPassword.value.trim() !== '') ||
          (newPassword && newPassword.value.trim() !== '') ||
          (confirmPassword && confirmPassword.value.trim() !== '')  ) {
        newPassword?.setValidators([Validators.required, Validators.minLength(8)]);
        confirmPassword?.setValidators([Validators.required, Validators.minLength(8)]);
        currentPassword?.setValidators([Validators.required]);
      } else {
        newPassword?.clearValidators();
        confirmPassword?.clearValidators();
        currentPassword?.clearValidators();
      }
  
      newPassword?.updateValueAndValidity({ onlySelf: true });
      confirmPassword?.updateValueAndValidity({ onlySelf: true });
  
      return null;
    };
  }

mustMatch(controlName: string, matchingControlName: string): ValidatorFn {
  return (formGroup: AbstractControl): ValidationErrors | null => {
    const control = formGroup.get(controlName);
    const matchingControl = formGroup.get(matchingControlName);

    if (!control || !matchingControl) {
      return null; 
    }

    if (matchingControl.errors && !matchingControl.errors['mustMatch']) {
      
      return null;
    }

    if (control.value !== matchingControl.value) {
      matchingControl.setErrors({ mustMatch: true });
    } else {
      matchingControl.setErrors(null);
    }

    return null;
  };
}


  onSubmit()
  {
    if(this.userForm.valid)
    {
      this.userService.updateUser(this.userForm.value).subscribe({
        next: (data: UserResponse)=>{
          this.userForm.clear();
          this.authService.resetUserInfo();
          this.snackbarService.success("User info updated successfully")
        },
        error:(err: any)=>{
          if(this.userForm.value.newPassword)
          {
            this.snackbarService.error("Invalid credentials")
          }
          else
          {
            this.snackbarService.error("Failed to update user info")
          }
        }
      })
    }
  }
}
