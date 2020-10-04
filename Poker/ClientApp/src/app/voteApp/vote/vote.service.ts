import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import * as signalR from '@microsoft/signalr';
import { promise } from 'protractor';

@Injectable({
  providedIn: 'root'
})
export class VoteService {
  private connection: signalR.HubConnection;
  public errorMessage: string;

  constructor(private httpClient: HttpClient) {
    this.connection = this.CreateConnection('/PokingHub');
    this.connection.start().catch(err => this.errorMessage = err);
  }

  get GetConnection(): signalR.HubConnection {
    return this.connection;
  }

  private CreateConnection(path:string): signalR.HubConnection {
    return new signalR.HubConnectionBuilder()
      .withUrl(path)
      .build();
  }

  public StartConnection(): Promise<void> {
    return this.connection.start();
  }

  public SetEventOn(eventName: string, newMethod: (...args: any[]) => void): void {
    this.connection.on(eventName, newMethod);
  }

  public JoinSession(currentClient: string, sessionId:string): Promise<any> {
    return this.connection.invoke<any>("JoinSession", currentClient, sessionId);
  }

  public SetOpenToPublic(sessionId:string): void {
    this.connection.send("SetOpenToPublic", sessionId);
  }

  public ClearVotes(sessionId: string): void {
    this.connection.send("ClearVotes", sessionId);
  }

  public GetSession(sessionId: string): Observable<boolean> {
    return this.httpClient.get<boolean>('api/Vote/GetSession?sessionId=' + sessionId);
  }

  public UpdateCurrentClient(currentClient: string, sessionId: string): void {
    this.connection.send("UpdateCurrentClient", currentClient, sessionId);
  }
}
