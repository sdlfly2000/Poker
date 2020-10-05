import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { NzMessageService } from 'ng-zorro-antd/message';

import { VoteService } from './vote.service';
import { Vote } from '../models/vote.model'
import { Client } from '../models/client.model'

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html',
  styleUrls: ["./vote.component.css"]
})
export class VoteComponent {

  public joinForm!: FormGroup;
  public vote: Vote;

  public CurrentClient: Client;
  public SessionId: string;
  public SelectedPoint: string;

  public isVotesUpdated: boolean = false;

  constructor(
    private voteService: VoteService,
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private message: NzMessageService) {

    this.joinForm = this.formBuilder.group({
      userName: new FormControl(null, Validators.required)
    });

    this.SessionId = this.activatedRoute.snapshot.paramMap.get("sessionId");
    
    this.InitialEvents();

    this.IsSessionCreated(this.SessionId);
  }
  
  public JoinSession(): void {
    if (this.ValidateForm(this.joinForm)) {
      var currentClient = new Client();

      currentClient.ConnectionId = this.voteService.GetConnection.connectionId;
      currentClient.Vote = null;
      currentClient.UserName = this.joinForm.get('userName').value;
      currentClient.IsReady = false;

      this.voteService.JoinSession(JSON.stringify(currentClient), this.SessionId)
        .then((vote: string) => {
          this.CurrentClient = currentClient;
          this.vote = JSON.parse(vote);
        });
    }
  }

  public OnSelectPoint(): void {
    if (this.SelectedPoint != undefined) {
      this.CurrentClient.Vote = this.SelectedPoint;
      this.CurrentClient.IsReady = true;
      this.voteService.UpdateCurrentClient(JSON.stringify(this.CurrentClient), this.SessionId)
        .then((vote: string) => {
          if (vote != undefined) {
            this.vote = JSON.parse(vote);
          }
        });
    }
  }

  public ShowVotes(): void {
    this.voteService.SetOpenToPublic(this.SessionId);
  }

  public ClearVotes(): void {
    this.voteService.ClearVotes(this.SessionId);
  }

  public UpdateVote(): void {
    this.isVotesUpdated = true;
    this.voteService.GetVote(this.SessionId).subscribe(
      (vote: Vote) => {
        this.vote = vote;
      },
      (error) => {
        this.message.error(error);
      },
      () => {
        this.isVotesUpdated = false;
      });
  }

  private IsSessionCreated(sessionId: string): void {
    this.voteService.GetSession(sessionId).subscribe(
      (sessionId: boolean) => {
        if (!sessionId) {
          this.router.navigateByUrl("");
        }
      },
      (error) => {
        this.router.navigateByUrl("");
      });
  }

  private ValidateForm(form: FormGroup): boolean {
    for (const i in form.controls) {
      if (form.controls.hasOwnProperty(i)) {
        form.controls[i].markAsDirty();
        form.controls[i].updateValueAndValidity();
      }
    }

    return form.valid;
  }

  private InitialEvents(): void {
    this.voteService.SetEventOn("NewClientJoin",
      (vote: string) => {
        this.vote = JSON.parse(vote);
      });
  }
}
