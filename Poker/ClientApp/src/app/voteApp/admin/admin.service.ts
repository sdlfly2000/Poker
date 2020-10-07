import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Vote } from '../models/vote.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private ControllerUrl: string = "api/Vote/";

  constructor(
    private httpClient: HttpClient) {

  }

  public GetAllVotes(): Observable<Vote[]> {
    return this.httpClient.get<Vote[]>(this.ControllerUrl + "GetAllVotes");
  }

  public RemoveVote(sessionId: string): void {
    this.httpClient.get(this.ControllerUrl + "RemoveVote?sessionId=" + sessionId);
  }
}
