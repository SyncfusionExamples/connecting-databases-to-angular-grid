import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { EditService, FilterService, GridModule, PageService, GroupService, AggregateService, SortService, ToolbarService } from '@syncfusion/ej2-angular-grids';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, GridModule
  ],
  providers: [EditService, ToolbarService, AggregateService, GroupService, FilterService, SortService, PageService],
  bootstrap: [AppComponent]
})
export class AppModule { }
