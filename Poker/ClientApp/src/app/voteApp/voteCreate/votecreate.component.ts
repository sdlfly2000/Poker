import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { VoteService } from '../vote.service';
import { Client } from '../models/client.model';

@Component({
  selector: 'app-VoteCreate',
  templateUrl: './votecreate.component.html',
  styleUrls: ['./votecreate.component.css']
})
export class VoteCreateComponent implements OnInit {

  private VoteUrl: string = "vote/";

  public createForm!: FormGroup;
  public CurrentUserName: string;

  constructor(
    private voteService: VoteService,
    private router: Router,
    private formBuilder: FormBuilder) {

    this.createForm = this.formBuilder.group({
      userName: [null, [Validators.required]]
    });
  }

  ngOnInit(): void {

  }

  public CreateSession(): void {

    if (this.ValidateForm(this.createForm)) {
      var currentClient = new Client();

      currentClient.ConnectionId = this.voteService.GetConnection.connectionId;
      currentClient.UserName = this.CurrentUserName;
      currentClient.IsReady = false;

      this.voteService.CreateSession(JSON.stringify(currentClient)).then((sessionId: string) => {
        this.router.navigateByUrl(this.VoteUrl + sessionId);
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
}
