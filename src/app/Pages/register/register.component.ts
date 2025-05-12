import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { LoginService } from '../../../Services/LoginService/login.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  messageError: string = '';

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private loginService:LoginService
  ) {
    this.registerForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, {
      validators: this.passwordsMatchValidator
    });
  }
  redirectToLogin() {
    this.router.navigate(['/login']);
  }
  ngOnInit(): void {}

  passwordsMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirm = form.get('confirmPassword')?.value;
    return password === confirm ? null : { mismatch: true };
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.messageError = 'Veuillez corriger les erreurs du formulaire.';
      return;
    }

    const formData = new FormData();
    formData.append('firstName', this.registerForm.value.firstName);
    formData.append('lastName', this.registerForm.value.lastName);
    formData.append('email', this.registerForm.value.email);
    formData.append('password', this.registerForm.value.password);
    this.loginService.postRegister(formData).subscribe({
      next: (data) => {
        this.router.navigate(['/']); 
      },
      error: (err) => {
        console.error(err);
      
      }
    });
    
  }
}
