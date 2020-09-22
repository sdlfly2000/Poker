import { Component } from '@angular/core';

import { VoteService } from './vote.service';
import { Vote } from './models/vote.model'

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html'
})
export class VoteComponent {

  public vote: Vote;
  public isVote: boolean;

  public UserName: string;
  public Message: number;

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
    this.voteService.CreateOrGetSession(sessionId).then(
      (vote: string) => {
        this.vote = JSON.parse(vote);
        this.isVote = true;
    });
  }

  public SayHello(): void {
    this.voteService.Greeting().then((msg: number) => {
      this.Message = msg;
    })
  }

  private InitialEvents(): void {
    this.voteService.SetEventOn("messageReceived", (vote: Vote) => this.vote = vote);
  }
}
