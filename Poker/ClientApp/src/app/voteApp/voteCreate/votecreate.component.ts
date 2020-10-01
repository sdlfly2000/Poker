import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';

import { VoteCreateService } from './votecreate.service';
import { Vote } from "../models/vote.model";

@Component({
  selector: 'app-VoteCreate',
  templateUrl: './votecreate.component.html',
  styleUrls: ['./votecreate.component.css']
})
export class VoteCreateComponent implements OnInit {

  private VoteControllerUrl: string = 'vote/';
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

  this.voteCreateService.CreateSession()
      .then((vote: Vote) => {
        this.router.navigateByUrl(this.VoteControllerUrl + vote.SessionId);
      });
  }
}
