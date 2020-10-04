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

import { IconDefinition } from '@ant-design/icons-angular';
import { NzIconModule, NZ_ICON_DEFAULT_TWOTONE_COLOR, NZ_ICONS } from 'ng-zorro-antd/icon';
import { CheckCircleTwoTone, CloseCircleTwoTone } from '@ant-design/icons-angular/icons';
const icons: IconDefinition[] = [CheckCircleTwoTone, CloseCircleTwoTone];

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
    NgZorroAntdModule,
    NzIconModule
  ],
  providers: [
    { provide: NZ_ICON_DEFAULT_TWOTONE_COLOR, useValue: '#00ff00' },
    { provide: NZ_ICONS, useValue: icons }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
