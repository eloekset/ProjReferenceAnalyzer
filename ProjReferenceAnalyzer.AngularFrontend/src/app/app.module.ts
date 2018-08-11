import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { VisModule } from 'ng2-vis';

import { AppComponent } from './app.component';
import { VisnetworkComponent } from './visnetwork/visnetwork.component';

@NgModule({
  declarations: [
    AppComponent,
    VisnetworkComponent
  ],
  imports: [
    BrowserModule,
    VisModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
