import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  constructor(private http: HttpClient) {

  }

  user: any = null;

  async loadUser() {
    const user = await firstValueFrom(
      this.http.get<any>("/api/user")
    )

    if ('user_id' in user) {
      this.user = user
    }

    return user;
  }

  login(loginForm: any) {
    return this.http.post<any>("/api/login", loginForm, {withCredentials: true})
      .subscribe(_ => {
        this.loadUser()
      })
  }

  register(registerForm: any) {
    return this.http.post<any>("/api/register", registerForm, {withCredentials: true})
      .subscribe(_ => {
        this.loadUser()
      })
  }

  logout() {
    return this.http.get("/api/logout")
      .subscribe(_ => this.user = null)
  }
}
