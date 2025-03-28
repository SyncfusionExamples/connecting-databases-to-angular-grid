import { Component, ViewChild } from '@angular/core';
import { DataManager } from '@syncfusion/ej2-data';
import { CustomAdaptor } from './CustomAdaptor';
import { EditService, FilterService, EditSettingsModel, GridModule, GridComponent, ToolbarItems, PageService, SortService, ToolbarService } from '@syncfusion/ej2-angular-grids';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: true,
  providers: [EditService, ToolbarService, PageService, SortService, FilterService],
  imports: [GridModule],
})
export class AppComponent {
  public data?: DataManager;

  @ViewChild('grid')
  public grid?: GridComponent;
  public editSettings?: EditSettingsModel;
  public toolbar?: ToolbarItems[];
  public employeeIDRules?: Object;
  public customerIDRules?: Object;
  public freightRules?: Object;
  public shipCityRules?: Object;

  ngOnInit(): void {
    this.data = new DataManager({
      url: 'https://localhost:7191/api/Grid',
      insertUrl: 'https://localhost:7191/api/Grid/Insert',
      updateUrl: 'https://localhost:7191/api/Grid/Update',
      removeUrl: 'https://localhost:7191/api/Grid/Remove',
      // Enable batch URL when batch editing is enabled.
      //batchUrl: 'https://localhost:7191/api/Grid/BatchUpdate',
      adaptor: new CustomAdaptor()
    });
    this.employeeIDRules = { required: true, number: true };
    this.customerIDRules = { required: true };
    this.freightRules = { required: true, min: 1, max: 1000 };
    this.shipCityRules = { required: true };
    this.toolbar = ['Add', 'Update', 'Delete', 'Cancel', 'Search'];
    this.editSettings = { allowAdding: true, allowDeleting: true, allowEditing: true, mode: 'Normal' };
  }
}
