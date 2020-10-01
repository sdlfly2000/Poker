import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Client } from '../models/client.model';
import { Vote } from "../models/vote.model";

@Injectable({
  providedIn: 'root'
})
export class VoteCreateService {
  private CreateSeesionUrl: string = 'api/Vote/CreateSession';

  constructor(private httpClient: HttpClient) {

  }

  public CreateSession(): Promise<Vote> {
    return this.httpClient.get<Vote>(this.CreateSeesionUrl).toPromise();
  }
}
