import { Component, OnInit } from '@angular/core';
import { BreadScrumComponent } from '../../building-blocks/bread-scrum/bread-scrum.component';
import { ButtonComponent } from '../../building-blocks/button/button.component';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { UserResponse } from '../../interfaces/user';
import { SnackbarService } from '../../services/snackbar.service';
import { StoreInfo } from '../../interfaces/storeinfo';
import { StoreInfoService } from '../../services/store-info.service';

@Component({
  selector: 'app-contact',
  imports: [BreadScrumComponent,
    ButtonComponent,
    ReactiveFormsModule
  ],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.css'
})
export class ContactComponent implements OnInit {
  contactForm: any;
  storeInfo: StoreInfo = {} as StoreInfo;
  constructor(private formGroup: FormBuilder,
    private readonly snackbarService: SnackbarService,
    private readonly storeInfoService: StoreInfoService,
    private readonly userService: UserService)
  {
    this.contactForm = this.formGroup.group({
      name: ["", Validators.required],
      email: ["", Validators.required],
      phoneNumber: ["", [
        Validators.required, 
        Validators.pattern(/^\+?[0-9\s\-]{10,20}$/), 
        Validators.minLength(10)
      ]],
      message: ["", [
        Validators.required,
        Validators.minLength(25) 
      ]]
    })
  }

  get phone() {
    return this.contactForm.get('phone');
  }
  
  ngOnInit(): void {
    this.setFormValues();
    this.storeInfoService.storeInfo$.subscribe({
      next: (data:StoreInfo) => {
        this.storeInfo = data;
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }
  
  setFormValues()
  {
    this,this.userService.userInfo$.subscribe({
      next: (data: UserResponse | null)=>{
        if(data)
        {
          this.contactForm.get('name')?.setValue(`${data.firstName} ${data.lastName}`);
          this.contactForm.get('email')?.setValue(data.email);
        }
      }
    })
  }

  onSubmit()
  {
    if(this.contactForm.valid)
    {
      this.userService.contact(this.contactForm.value).subscribe({
        next: ()=>{
          this.snackbarService.info("Contact mail sent to support team");
        },
        error: (err:any)=>{
          this.snackbarService.error("Failed to contact support team");
        }
      })
    }
  }
}
