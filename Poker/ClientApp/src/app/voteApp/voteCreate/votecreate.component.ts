import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';

import { Client } from '../models/client.model';
import { VoteCreateService } from './votecreate.service';

@Component({
  selector: 'app-VoteCreate',
  templateUrl: './votecreate.component.html',
  styleUrls: ['./votecreate.component.css']
})
export class VoteCreateComponent implements OnInit {

  private VoteControllerUrl: string = 'vote/'
  public createForm!: FormGroup;

  constructor(
    private voteCreateService: VoteCreateService,
    private router: Router,
    private formBuilder: FormBuilder) {

    this.createForm = this.formBuilder.group({
      userName: new FormControl(null, Validators.required)
    });
  }

  ngOnInit(): void {

  }

  public CreateSession(): void {

    if (this.ValidateForm(this.createForm)) {
      var currentClient = new Client();

      currentClient.ConnectionId = null;
      currentClient.Vote = null;
      currentClient.UserName = this.createForm.get('userName').value;
      currentClient.IsReady = false;

      this.voteCreateService.CreateSession(currentClient)
        .then((sessionId) => {
          this.router.navigateByUrl(this.VoteControllerUrl + sessionId);
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
