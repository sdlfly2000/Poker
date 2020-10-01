import { Injectable } from '@angular/core';

import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class VoteService {
  private connection: signalR.HubConnection;
  public errorMessage: string;

  constructor() {
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

  public Send(messageName: string): Promise<void> {
    return this.connection.send(messageName);
  }

  public JoinSession(currentClient: string, sessionId:string): Promise<any> {
    return this.connection.invoke<any>("JoinSession", currentClient, sessionId);
  }

  public GetSession(sessionId?: string): Promise<any> {
    return this.connection.invoke<any>("GetSession", sessionId);
  }
}
