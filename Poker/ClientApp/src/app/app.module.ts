import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgZorroAntdModule } from 'ng-zorro-antd';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { VoteComponent } from './voteApp/vote/vote.component';
import { VoteCreateComponent } from './voteApp/voteCreate/votecreate.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    VoteComponent,
    VoteCreateComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: VoteCreateComponent, pathMatch: 'full' },
      { path: 'vote/:sessionId', component: VoteComponent, pathMatch: 'full' }
    ]),
    NgZorroAntdModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
