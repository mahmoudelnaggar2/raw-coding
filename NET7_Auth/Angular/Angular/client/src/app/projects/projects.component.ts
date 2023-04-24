import { Component } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.css']
})
export class ProjectsComponent {
  constructor(private http: HttpClient) {
  }

  projects: any = []
  userId: any = ""

  async ngOnInit(){
    this.projects = await firstValueFrom(
      this.http.get('/api/projects')
    )
  }

  promote() {
    return firstValueFrom(
      this.http.post(`/api/promote/${this.userId}`, null)
    )
  }

  addUser(projectId: any) {
    return firstValueFrom(
      this.http.post(`/api/projects/${projectId}/add-user/${this.userId}`, null)
    )
  }
}
