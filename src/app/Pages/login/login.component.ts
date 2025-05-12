import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginService } from '../../../Services/LoginService/login.service';
import { NgIf } from '@angular/common';
import {Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone:true,
  imports: [ReactiveFormsModule,NgIf,RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  messageError:string='';
  constructor(private fb: FormBuilder,private loginService:LoginService,private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }
  ngOnInit(): void {
  }

  onSubmit() {
    debugger
    if (this.loginForm.valid) {
      const formData = new FormData();
      formData.append('email', this.loginForm.get('email')?.value);
      formData.append('password', this.loginForm.get('password')?.value);
  
      this.loginService.postData(formData).subscribe({
        next: (data) => {
          debugger
          this.router.navigate(['/validate-code']); 
        },
        error: (error) => {
          this.messageError = error.error?.message || 'Une erreur est survenue lors de la connexion.';
        }
      });
    }
  }
  
}
