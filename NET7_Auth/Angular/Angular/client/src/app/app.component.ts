import {Component} from '@angular/core';
import {AuthService} from "./auth.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'client';

  constructor(private auth: AuthService) {
  }

  username: any;

  async ngOnInit() {
    const user = await this.auth.loadUser()
    this.username = user['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
  }

  logout() {
    return this.auth.logout()
  }
}
