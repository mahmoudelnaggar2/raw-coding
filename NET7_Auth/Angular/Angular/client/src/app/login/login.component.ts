import {Component} from '@angular/core';
import {AuthService} from "../auth.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private auth: AuthService) {
  }

  username: any = "";
  password: any = "";

  login() {
    return this.auth.login({
      username: this.username,
      password: this.password
    })
  }
}
