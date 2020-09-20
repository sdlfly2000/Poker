import { Component } from '@angular/core';


import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html'
})
export class VoteComponent {

  private connection: signalR.HubConnection;

  public UserName: string;
  public Message: string;

  public InputUserName: string;
  public InputMessage: string;

  public ErrorMessage: string;

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('/hub')
      .build();

    this.connection.on('messageReceived', (userName: string, message: string) => {
      this.UserName = userName;
      this.Message = message;
    });

    this.connection.start().catch(err => this.ErrorMessage = err);
  }

  public SendMessage(): void {
    this.connection.send("NewMessage", this.InputUserName, this.InputMessage)
      .then(() => {
        this.InputMessage = "";
        this.InputUserName = "";
      });
  }
}
