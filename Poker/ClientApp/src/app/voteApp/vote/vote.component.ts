import { Component } from '@angular/core';

import { VoteService } from '../vote.service';
import { IVote } from '../models/vote.model'
import { IClient, Client } from '../models/client.model'

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html',
  styleUrls: ["./vote.component.css"
  ]
})
export class VoteComponent {

  public CurrentClient: IClient

  public vote: IVote;
  public isVote: boolean;

  public UserName: string;
  public Message: number;

  public InputUserName: string;
  public InputMessage: string;

  public ErrorMessage: string;

  constructor(private voteService: VoteService) {
    this.InitialEvents();
    this.voteService.StartConnection();

    this.CurrentClient = new Client();
    this.CurrentClient.ConnectionId = this.voteService.GetConnection.connectionId;
  }

  public GetSession(sessionId:string): void {
    this.voteService.GetSession(sessionId).then(
      (vote: string) => {
        this.vote = JSON.parse(vote);
        this.isVote = true;
    });
  }

  private InitialEvents(): void {
    this.voteService.SetEventOn("messageReceived", (vote: IVote) => this.vote = vote);
  }
}
