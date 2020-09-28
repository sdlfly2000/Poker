import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Client } from '../models/client.model';

@Injectable({
  providedIn: 'root'
})
export class VoteCreateService {
  private CreateSeesionUrl: string = 'api/Vote/CreateSession';

  constructor(private httpClient: HttpClient) {

  }

  public CreateSession(client: Client): Promise<string> {
    return this.httpClient.post<string>(this.CreateSeesionUrl, client).toPromise<string>();
  }
}
