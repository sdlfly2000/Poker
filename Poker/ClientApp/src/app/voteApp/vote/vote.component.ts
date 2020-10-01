import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { VoteService } from '../vote.service';
import { IVote, Vote } from '../models/vote.model'
import { IClient, Client } from '../models/client.model'

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html',
  styleUrls: ["./vote.component.css"]
})
export class VoteComponent {

  public joinForm!: FormGroup;
  public vote: Vote;
  public CurrentClient: Client;

  public SeesionId: string;

  public UserName: string;
  public Message: number;

  public InputUserName: string;
  public InputMessage: string;

  public ErrorMessage: string;

  constructor(
    private voteService: VoteService,
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute) {

    this.joinForm = this.formBuilder.group({
      userName: new FormControl(null, Validators.required)
    });

    this.SeesionId = this.activatedRoute.snapshot.paramMap.get("sessionId");
    
    this.InitialEvents();
  }
  
  public JoinSession(): void {
    if (this.ValidateForm(this.joinForm)) {
      var currentClient = new Client();

      currentClient.ConnectionId = this.voteService.GetConnection.connectionId;
      currentClient.Vote = null;
      currentClient.UserName = this.joinForm.get('userName').value;
      currentClient.IsReady = false;

      this.voteService.JoinSession(JSON.stringify(currentClient), this.SeesionId)
        .then((vote: string) => {
          this.CurrentClient = currentClient;
          this.vote = JSON.parse(vote);
        });
    }
  }

  private ValidateForm(form: FormGroup): boolean {
    for (const i in form.controls) {
      form.controls[i].markAsDirty();
      form.controls[i].updateValueAndValidity();
    }

    return form.valid;
  }

  private InitialEvents(): void {
    this.voteService.SetEventOn("messageReceived", (vote: IVote) => this.vote = vote);
  }
}
