import { Component } from '@angular/core';
import { Vote } from '../models/vote.model';
import { AdminService } from './admin.service';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-voteadmin',
  templateUrl: './admin.component.html'
})
export class AdminComponent {

  public votes: Vote[];

  constructor(
    private adminService: AdminService,
    private message: NzMessageService) {

    this.GetVotes();

  }

  public GetVotes(): void {
    this.adminService.GetAllVotes().subscribe(
      (votes: Vote[]) => {
        this.votes = votes;
      },
      (error) => {
        this.message.error(error);
      }
    )
  }

}
