import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { CompleteRegistrationRequest } from '../../interfaces/verify';

@Component({
  selector: 'app-verify-email',
  imports: [
    RouterModule
  ],
  templateUrl: './verify-email.component.html',
  styleUrl: './verify-email.component.css'
})
export class VerifyEmailComponent implements OnInit {
  verificationId: string|null = "";
  isVerified: boolean =  false;
  constructor(private activatedRoute: ActivatedRoute, private route: Router, private readonly userService: UserService)
  {

  }
  
  ngOnInit(): void {
    this.verificationId = this.activatedRoute.snapshot.paramMap.get('id');
    if(this.verificationId)
    {
      const request = { uniqueId: this.verificationId } as CompleteRegistrationRequest;
      this.userService.completeRegistration(request).subscribe({
        next: (data: any) => {
          this.isVerified = true;
        },
        error: (err) => {
          console.error(err.message);
        }
      });
    }
  }
}
