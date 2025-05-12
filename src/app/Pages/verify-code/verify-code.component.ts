import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';
import { LoginService } from '../../../Services/LoginService/login.service';

@Component({
  selector: 'app-verify-code',
  imports: [ReactiveFormsModule,NgIf],
  templateUrl: './verify-code.component.html',
  styleUrl: './verify-code.component.css'
})
export class VerifyCodeComponent {
  codeForm: FormGroup;
  errorMessage: string = '';

  constructor(private fb: FormBuilder, private loginService:LoginService, private router: Router) {
    this.codeForm = this.fb.group({
      code0: [''], code1: [''], code2: [''],
      code3: [''], code4: [''], code5: ['']
    });
  }

  onInput(index: number, event: any) {
    const input = event.target;
    const value = input.value;
    if (value && input.nextElementSibling) {
      input.nextElementSibling.focus();
    }

    const fullCode = Object.values(this.codeForm.value).join('');
    if (fullCode.length === 6) {
      this.verifyCode(fullCode);
    }
  }

  onKeyDown(index: number, event: KeyboardEvent) {
    const input = event.target as HTMLInputElement;
    if (event.key === 'Backspace' && !input.value && input.previousElementSibling) {
      (input.previousElementSibling as HTMLInputElement)?.focus();
    }
  }

  verifyCode(code: string) {
    if (code) {
      const formData = new FormData();
      formData.append("Code", code);

      this.loginService.postVerifyData(formData).subscribe({
        next: (response: any) => {
          if (response.isSucces) {
            this.router.navigate(['/']);
          } else {
            this.errorMessage = response.message;
          }
        },
        error: err => {
          this.errorMessage = 'Erreur de vérification.';
        }
      });
    }
  }
}
