import { Component } from '@angular/core';

import { VoteService } from './vote.service';
import { Vote } from './models/vote.model'

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html'
})
export class VoteComponent {

  public vote: Vote;

  public UserName: string;
  public Message: string;

  public InputUserName: string;
  public InputMessage: string;

  public ErrorMessage: string;

  constructor(private voteService: VoteService) {
    this.InitialEvents();
    this.voteService.StartConnection();
  }

  public SendMessage(): void {
    this.voteService.Send("NewMessage")
      .then(() => {
        this.InputMessage = "";
        this.InputUserName = "";
      });
  }

  public CreateOrGetSession(sessionId:string): void {
    this.voteService.CreateOrGetSession(sessionId).then((vote: Vote) => {
       this.vote = vote;
    });
  }

  private InitialEvents(): void {
    this.voteService.SetEventOn("messageReceived", (vote: Vote) => this.vote = vote);
  }
}
