import { Injectable } from '@angular/core';
import { Vote } from './models/vote.model';

import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class VoteService {
  private connection: signalR.HubConnection;
  public errorMessage: string;

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/hub')
      .build();
  }

  public GetConnection(): signalR.HubConnection {
    return this.connection;
  }

  public SetEventOn(eventName: string, newMethod: (...args: any[]) => void): void {
    this.connection.on(eventName, newMethod);
  }

  public StartConnection(): void {
    this.connection.start().catch(err => this.errorMessage = err);
  }

  public Send(messageName: string): Promise<void> {
    return this.connection.send(messageName);
  }

  public CreateOrGetSession(sessionId: string): Promise<Vote> {
    return this.connection.invoke<Vote>("CreateOrGetSession", sessionId);
  }
}
