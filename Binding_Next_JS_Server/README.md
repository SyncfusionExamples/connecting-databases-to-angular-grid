---
layout: post
title: Data binding with Next.js server in Syncfusion Angular Grid
description: Learn integrating the Syncfusion Angular Grid with Next.js using the custom binding for CRUD and data operation.
control: Next.js
platform: ej2-angular
documentation: ug
domainurl: ##DomainURL##
---

# Building a Next.js Application with Syncfusion Angular Grid

[Next.js](https://nextjs.org/) is a powerful React framework designed for building full-stack web applications. It includes built-in features such as server-side rendering, automatic code splitting, intuitive routing, and API routes, providing a solid foundation for developing modern, high-performance applications.

This guide demonstrates integration with the Syncfusion Angular Grid in a hybrid setup, where the Angular Grid runs client-side (e.g., in an Angular micro-frontend or standalone component) and fetches data from Next.js server endpoints.

## Prerequisites

  - Node.js: LTS version (e.g., v20.x or later).

  - npm/yarn: For package management.

  - Angular CLI: For creating/serving the Angular part (if separate).

## Building the Next.js application

* Open your terminal or command prompt and run the following command to create a new folder **nextjs_grid**.

```bash
npm create next-app@latest next_js_server
cd next_js_server
```

The **nextjs_grid** folder is created with the default Next.js application structure.

## Configuring Next.js server

Next.js Route handlers provide a modern way to create server‑side endpoints directly inside the App Router, using the Web‑standard request and response APIs. They allow you to execute backend logic such as data processing, CRUD operations, and custom APIs without needing a separate server layer. Route Handlers are defined in the **route.ts** file inside the **app** directory which allows you to create custom request handlers for a given route. To populate the health care data entries, follow the below steps:

**Step 1:** Create a new route file (**api/health_care/route.ts**) to add the server-side data implementations.

**Step 2:** Create a new database file (**data/health_care_Entities.ts**) to store the data.

**Step 3:** Inside the **route.ts** file, add a `GET` method to return the data to the client when a request is sent. Ensure the response follows a structured format that includes both the current view dataset and the total data count. This approach supports on‑demand data loading and enables the client to handle operations such as paging or filtering effectively when using Syncfusion data binding approaches.

The required response format includes:
  - **result**: The list of data to be displayed in the current Grid view.
  - **count**: The total number of records available in the dataset.

```typescript
import { NextResponse, NextRequest } from "next/server";
import { doctorDetails } from '../../data/health_care_Entities';

// GET - Retrieve all data
export async function GET(request: NextRequest) {

    const count = doctorDetails.length;
    const result = doctorDetails;

    return NextResponse.json({ result, count });
}
```

Now a server side API for fetching the "health_care_Entities" is implemented using the `GET` method.

## Creating a Angular client application

Create a new Angular application using below Angular CLI command.

```bash
ng new angular_client
```

## Adding Syncfusion packages

Install the necessary Syncfusion packages using the below command.

```bash
npm install @syncfusion/ej2-angular-grids --save
npm install @syncfusion/ej2-data --save
```

Once the dependencies are installed, the required CSS files are available in the (**../node_modules/@syncfusion**) package directory, and add the required CSS reference files in the (**src/style.css**) file.

```css
[global.css]

@import '../node_modules/@syncfusion/ej2-base/styles/material3.css';  
@import '../node_modules/@syncfusion/ej2-buttons/styles/material3.css';  
@import '../node_modules/@syncfusion/ej2-calendars/styles/material3.css';  
@import '../node_modules/@syncfusion/ej2-dropdowns/styles/material3.css';  
@import '../node_modules/@syncfusion/ej2-inputs/styles/material3.css';  
@import '../node_modules/@syncfusion/ej2-navigations/styles/material3.css';
@import '../node_modules/@syncfusion/ej2-popups/styles/material3.css';
@import '../node_modules/@syncfusion/ej2-splitbuttons/styles/material3.css';
@import '../node_modules/@syncfusion/ej2-notifications/styles/material3.css';
@import '../node_modules/@syncfusion/ej2-angular-grids/styles/material3.css';
```

The above example uses the "material3" theme. You can replace "material3" with other available [themes](https://ej2.syncfusion.com/angular/documentation/appearance/theme-studio) based on your application requirements.

## Integrating Syncfusion Angular Grid with Next.js

Syncfusion Angular Grid supports [custom binding](https://ej2.syncfusion.com/angular/documentation/grid/data-binding/remote-data#custom-binding) enabling interaction with any API service. Server-side logic processes data and returns results to the Grid, giving full control over application-specific operations.

The Grid expects a response object with `result` and `count` for proper binding. Data actions trigger the [dataStateChange](https://ej2.syncfusion.com/angular/documentation/api/grid/index-default#datastatechange) event, while CRUD operations trigger [dataSourceChanged](https://ej2.syncfusion.com/angular/documentation/api/grid/index-default#datasourcechanged), allowing server-side handling of Grid interactions.

In **src/app.ts**, configure the Grid with custom binding logic. Define a function "fetchData" to send Grid state to the Next.js API and bind the response. Use `ngOnInit` to load the initial dataset.

```ts
[src/app.ts]

import { Component, signal, ViewChild } from '@angular/core';
import { DataSourceChangedEventArgs, DataStateChangeEventArgs, EditService, GridComponent, GridModule, ToolbarService } from '@syncfusion/ej2-angular-grids'
import { PageService, SortService, FilterService, GroupService } from '@syncfusion/ej2-angular-grids'
import { DataManager, Query } from '@syncfusion/ej2-data';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,  GridModule, CommonModule, ButtonModule],
  providers: [PageService, SortService, FilterService, EditService, ToolbarService, GroupService],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

export class App {
// Fetch data from server with current state
async fetchData(gridState: any) {
  // Convert gridState to URL-encoded string
  const encodedState = encodeURIComponent(JSON.stringify(gridState));
  const url = `http://localhost:3001/api/health_care?gridState=${encodedState}`;

  const response = await fetch(url, {
    method: 'GET',
    headers: { 'Content-Type': 'application/json' },
  });
  return await response.json();
}

// Load initial data when component mounts
  ngOnInit(): void {
    const initialState = {
      skip: 0,
      take: 12,
      sorted: [],
      where: [],
      search: [],
    };
    this.fetchData(initialState).then((res) => {
      this.data = res;
    });
  }
  ```

```html
[src/app.html]
  <ejs-grid #grid [dataSource]="data" (dataStateChange)="dataStateChange($event)">
    <e-columns>
      <e-column field="DoctorId" headerText="Doctor ID" width="120" isPrimaryKey="true"
        [validationRules]="{ required: true }"></e-column>
          {/* Include additional columns here */}
    </e-columns>
  </ejs-grid>
</div>

```

## Performing data operation

In a Next.js  server, the Syncfusion [DataManager](https://ej2.syncfusion.com/angular/documentation/data/getting-started) efficiently handles data operations such as filtering, sorting, searching, paging, and aggregation. It processes the Syncfusion [Query](https://ej2.syncfusion.com/angular/documentation/data/querying), which specifies all operation details, and executes them directly against the data source. By streamlining these tasks, DataManager ensures consistent, accurate results and significantly reduces development effort and time.

Inside the **api/health_care/route.ts** file, import the `DataManager` and `Query` from the `@syncfusion/ej2-data` package to implement the data operations using the Syncfusion `DataManager`.

### Filtering
 
Enable the Grid with filtering by setting the [allowFiltering](https://ej2.syncfusion.com/angular/documentation/api/grid/index-default#allowfiltering) property to "true".
 
Filtering the Grid triggers the `dataStateChange` event with the required filter details as `where` property. Using this property call send fetch request to the server inside the "fetchData" method. On the server side, these filter parameters are translated into a [filter query](https://ej2.syncfusion.com/angular/documentation/data/querying#filtering) and executed through the `DataManager` to get the filtered data.

The image illustrates the filter state passed to the `where` property of the `dataStateChange` event arguments.

![Next_JS_Filtering](../images/next_js_filter.png)

Code example for handling filter action inside the server (**route.ts**) file.

```typescript
import { Predicate } from '@syncfusion/ej2-data';
import { NextResponse, NextRequest } from "next/server";
import { DataManager, Query } from '@syncfusion/ej2-data';
import { doctorDetails } from '../../data/health_care_Entities';

// GET - Retrieve the resultant data
export async function GET(request: NextRequest) {

    const gridStateParam = new URL(request.url).searchParams.get('gridState');
    const gridState = JSON.parse(decodeURIComponent(gridStateParam));
    const query = new Query();

    // Filtering
    if (gridState.where && Array.isArray(gridState.where) && gridState.where.length > 0) {
        performFiltering(gridState.where, query);
    }

    // Execute query on data
    const resultantData = new DataManager(doctorDetails).executeLocal(query);
    const count: any = resultantData.length;
    let result: any = resultantData

    return NextResponse.json({ result, count });
}

```

To handle complex filter conditions, where multiple predicates are combined using logical operators such as and or or, use the following helper functions inside the server (**route.ts**) file.

```typescript

// Normalize condition string (default to 'and')
const normalize = (condition?: string) => (condition || 'and').toLowerCase();

// Recursively build predicate tree
const buildPredicate = (node: any, ignoreCase: boolean): any =>
    node?.isComplex && node.predicates?.length
        ? node.predicates
            .map((p: any) => buildPredicate(p, ignoreCase))
            .filter(Boolean)
            .reduce((acc: any, cur: any) =>
                acc ? (normalize(node.condition) === 'or' ? acc.or(cur) : acc.and(cur)) : cur, null)
        : (node?.field && node?.operator ? new Predicate(node.field, node.operator, node.value, ignoreCase) : null);

// Apply filtering based on predicates
const performFiltering = (input: any, query: any) => {
    const filter = Array.isArray(input) ? input[0] : input;
    if (!filter?.predicates?.length) return;
    const ignoreCase = filter.ignoreCase !== undefined ? !!filter.ignoreCase : true;
    const condition = normalize(filter.condition);
    const combined = filter.predicates
        .map((p: any) => buildPredicate(p, ignoreCase))
        .filter(Boolean)
        .reduce((acc: any, cur: any) => acc ? (condition === 'or' ? acc.or(cur) : acc.and(cur)) : cur, null);
    if (combined) query.where(combined);
};

```

Code example for handling filter action inside the client (**src/app.ts**) file.

```ts
[src/app.ts]

import {FilterService} from '@syncfusion/ej2-angular-grids'

@Component({
  providers: [FilterService],
})

export class App {
  public filterSettings: Object = { type: 'Excel' };
  async dataStateChange(args: DataStateChangeEventArgs) {
    const gridState = {
      skip: args.skip,
      take: args.take,
      sorted: args.sorted,
      where: args.where,
      search: args.search,
    };

    const res: any = await this.fetchData(gridState);

    if (
      args.action &&
      (args.action.requestType === 'filterchoicerequest' ||
        args.action.requestType === 'filtersearchbegin' ||
        args.action.requestType === 'stringfilterrequest')
    ) {
      (args as any).dataSource(res.result);
    } else {
      this.data = res;
    }
  }
}
```

```html
[src/app.html]
  <ejs-grid #grid [dataSource]="data" [allowFiltering]="true"
    [filterSettings]="filterSettings"(dataStateChange)="dataStateChange($event)">
    <e-columns>
      <e-column field="DoctorId" headerText="Doctor ID" width="120" isPrimaryKey="true"
        [validationRules]="{ required: true }"></e-column>
          {/* Include additional columns here */}
    </e-columns>
  </ejs-grid>
</div>

```

### Searching

Searching in the Grid is enabled by including `Search` in the Grid’s toolbar items. When users perform a search, the `dataStateChange` event exposes the current search details through its `search` parameter. These values are then transformed on the server into a [search query](https://ej2.syncfusion.com/angular/documentation/data/querying#searching), and the DataManager executes the query to fetch the corresponding records.

The image illustrates the search state passed to the `dataStateChange` event arguments.

![Next_JS_Searching](../images/next_js_search.png)

Code example for handling search action inside the server (**route.ts**) file.
 
```typescript
import { Predicate } from '@syncfusion/ej2-data';
import { NextResponse, NextRequest } from "next/server";
import { DataManager, Query } from '@syncfusion/ej2-data';
import { doctorDetails } from '../../data/health_care_Entities';

// Helper function: Apply search functionality
const performSearching = (searchParam: any, query: any) => {
    const { fields, key, operator, ignoreCase } = searchParam[0];
    query.search(key, fields, operator, ignoreCase);
};

// GET - Retrieve the resultant data
export async function GET(request: NextRequest) {

    const gridStateParam = new URL(request.url).searchParams.get('gridState');
    const gridState = JSON.parse(decodeURIComponent(gridStateParam));
    const query = new Query();

    // Searching
    if (gridState.search && Array.isArray(gridState.search) && gridState.search.length > 0) {
        performSearching(gridState.search, query);
    }

    // Execute query on data
    const resultantData = new DataManager(doctorDetails).executeLocal(query);
    const count: any = resultantData.length;
    let result = resultantData;

    return NextResponse.json({ result, count });
}
```

Code example for handling search action inside the client (**src/app.ts**) file.

```ts
[src/app.ts]

import {ToolbarService} from '@syncfusion/ej2-angular-grids'

@Component({
  providers: [ToolbarService],
})

export class App {
  public toolbar: string[] = ['Search'];
  async dataStateChange(args: DataStateChangeEventArgs) {
    // Handle data actions here
  }
}
```

```html
[src/app.html]
  <ejs-grid #grid [dataSource]="data" [toolbar]="toolbar" (dataStateChange)="dataStateChange($event)">
    <e-columns>
      <e-column field="DoctorId" headerText="Doctor ID" width="120" isPrimaryKey="true"
        [validationRules]="{ required: true }"></e-column>
          {/* Include additional columns here */}
    </e-columns>
  </ejs-grid>
</div>

```

### Sorting

Sorting in the Grid is enabled by setting the [allowSorting](https://ej2.syncfusion.com/angular/documentation/api/grid/index-default#allowsorting) property to "true". When a sort action occurs, the `dataStateChange` event exposes the sorting details through its `sort` parameter. On the server, these details are converted into a [sorting query](https://ej2.syncfusion.com/angular/documentation/data/querying#sorting), and the DataManager executes the query to return the ordered records.

The image illustrates the sorted state passed to the `dataStateChange` event arguments.

![Next_JS_Sorting](../images/next_js_sort.png)

Code example for handling sort action inside the server (**route.ts**) file.

```typescript
import { Predicate } from '@syncfusion/ej2-data';
import { NextResponse, NextRequest } from "next/server";
import { DataManager, Query } from '@syncfusion/ej2-data';
import { doctorDetails } from '../../data/health_care_Entities';

// Helper function: Apply sorting
const performSorting = (sortArray: any[], query: any) => {
    for (let i = 0; i < sortArray.length; i++) {
        const { name, direction } = sortArray[i];
        query.sortBy(name, direction);
    }
};

// GET - Retrieve the resultant data
export async function GET(request: NextRequest) {

    const gridStateParam = new URL(request.url).searchParams.get('gridState');
    const gridState = JSON.parse(decodeURIComponent(gridStateParam));
    const query = new Query();

    // Sorting
    if (gridState.sorted && Array.isArray(gridState.sorted) && gridState.sorted.length > 0) {
        performSorting(gridState.sorted, query);
    }

    // Execute query on data
    const resultantData = new DataManager(doctorDetails).executeLocal(query);
    const count: any = resultantData.length;
    let result: any = resultantData;

    return NextResponse.json({ result, count });
}

```

Code example for handling sort action inside the client (**src/app.ts**) file.

```ts
[src/app.ts]

import {SortService} from '@syncfusion/ej2-angular-grids'

@Component({
  providers: [SortService],
})

export class App {
  async dataStateChange(args: DataStateChangeEventArgs) {
    // Handle data actions here
  }
}
```

```html
[src/app.html]
  <ejs-grid #grid [dataSource]="data" [allowSorting]="true" (dataStateChange)="dataStateChange($event)">
    <e-columns>
      <e-column field="DoctorId" headerText="Doctor ID" width="120" isPrimaryKey="true"
        [validationRules]="{ required: true }"></e-column>
          {/* Include additional columns here */}
    </e-columns>
  </ejs-grid>
</div>

```

### Paging

Enable paging in the Grid by setting the [allowPaging](https://ej2.syncfusion.com/angular/documentation/api/grid/index-default#allowpaging) property "true" .

When a pagination action occurs, the `dataStateChange` event is triggered, providing the paging details through the `skip` and `take` parameters. On the server side, these values are translated into a [paging query](https://ej2.syncfusion.com/angular/documentation/data/querying#paging) and processed by the `DataManager`, ensuring that records are returned in segmented pages. This enables efficient navigation across large datasets.

The accompanying image demonstrates how the `skip` and `take` parameters are passed within the `dataStateChange` event arguments.

![Next_JS_Paging](../images/next_js_page.png)

Code example for handling paging action inside the server (**route.ts**) file.

```typescript
import { Predicate } from '@syncfusion/ej2-data';
import { NextResponse, NextRequest } from "next/server";
import { DataManager, Query } from '@syncfusion/ej2-data';
import { doctorDetails } from '../../data/health_care_Entities';

// Helper function: Apply paging
const performPaging = (data: any[], gridState: any) => {
    if (!gridState.take || gridState.take <= 0) {
        return data;
    }
    const pageSkip = gridState.skip || 0;
    const pageSize = gridState.take;
    return data.slice(pageSkip, pageSkip + pageSize);
};

// GET - Retrieve the resultant data
export async function GET(request: NextRequest) {

    const gridStateParam = new URL(request.url).searchParams.get('gridState');
    const gridState = JSON.parse(decodeURIComponent(gridStateParam));
    const query = new Query();

    // Execute query on data
    const resultantData = new DataManager(doctorDetails).executeLocal(query);
    const count: any = resultantData.length;
    let result: any = resultantData;

    // Paging
    result = performPaging(result, gridState);

    return NextResponse.json({ result, count });
}

```

Code example for handling paging action inside the client (**src/app.ts**) file.

```ts
[src/app.ts]

import {PageService} from '@syncfusion/ej2-angular-grids'

@Component({
  providers: [PageService],
})

export class App {
  async dataStateChange(args: DataStateChangeEventArgs) {
    // Handle data actions here
  }
}
```

```html
[src/app.html]
  <ejs-grid #grid [dataSource]="data" [allowPaging]="true" (dataStateChange)="dataStateChange($event)">
    <e-columns>
      <e-column field="DoctorId" headerText="Doctor ID" width="120" isPrimaryKey="true"
        [validationRules]="{ required: true }"></e-column>
          {/* Include additional columns here */}
    </e-columns>
  </ejs-grid>
</div>

```
 
### CRUD operations

To allow editing operations within the Grid, configure editSettings by setting `allowEditing`, `allowAdding`, and `allowDeleting` to "true" and add the `dataSourceChanged` event to the Grid component.

When CRUD action is performed, the Grid triggers the `dataSourceChanged` event. This event sends the corresponding CRUD parameters to the server, to handle the appropriate create, update, or delete operations.


```ts
[src/app.ts]

export class App {
  // Handle CRUD operations
  async dataSourceChanged(args: DataSourceChangedEventArgs) {
        // Handle CRUD operations here 
  };

  }
  ```

```html
[src/app.html]
  <ejs-grid #grid [dataSource]="data" (dataSourceChanged)="dataSourceChanged($event)">
    <e-columns>
      <e-column field="DoctorId" headerText="Doctor ID" width="120" isPrimaryKey="true"
        [validationRules]="{ required: true }"></e-column>
          {/* Include additional columns here */}
    </e-columns>
  </ejs-grid>
</div>

```

**Insert operation**

Adding a new record to the database involves the following steps:

**Step 1:** Define the "POST" method in the **route.ts** file to handle creating a new product. This method accepts the new data from the client and insert it to the database.

```ts
// POST - Create a new data
export async function POST(request: NextRequest) {
    const body = await request.json();
    if (body.action === 'add') {
        const newDoctor: any = {
            DoctorId: body.DoctorId,
            Name: body.Name,
            Specialty: body.Specialty,
            Experience: body.Experience,
            Availability: body.Availability,
            Email: body.Email,
            Contact: body.Contact
        };
        doctorDetails.push(newDoctor);
        return NextResponse.json(newDoctor, { status: 201 });
    }
}
```

**Step 2:** Inside the `dataStateChange` event handler in the **page.tsx** file, send a POST request to the server based on its argument details. inside the request then function call the `endEdit` method from the argument.

The image illustrates the newly inserted data passed to the `dataSourceChanged` event arguments.

![Next_JS_Add](../images/next_js_add.png)

```ts
 // Handle CRUD operations
   async dataSourceChanged(args: DataSourceChangedEventArgs) {
      const response = fetch('/api/health_care', {
        'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ ...args.data, action: 'add' }),
      });

    if (response.ok) {
      const result = await response.json();
      args?.endEdit?.();
    }
  };

```

**Update operation**

Updating an existing record in the database involves the following steps:

**Step 1:** Define the "PUT" method in the **route.ts** file to handle updating an existing product. This method accepts the updated data from the client and add those changes to the database.

```ts
// PUT - Update an existing data
export async function PUT(request: NextRequest) {
    const body = await request.json();
    if (body.action === 'edit') {
        const doctorIndex = doctorDetails.findIndex(u => u.DoctorId === body.DoctorId);
        doctorDetails[doctorIndex] = {
            ...doctorDetails[doctorIndex],
            Name: body.name || doctorDetails[doctorIndex].Name,
            Specialty: body.Specialty || doctorDetails[doctorIndex].Specialty,
            Experience: body.Experience || doctorDetails[doctorIndex].Experience,
            Availability: body.Availability || doctorDetails[doctorIndex].Availability,
            Email: body.Email || doctorDetails[doctorIndex].Email,
            Contact: body.Contact || doctorDetails[doctorIndex].Contact
        };
        return NextResponse.json(doctorDetails[doctorIndex]);
    }
}
```

**Step 2:** Inside the `dataStateChange` event handler in the **page.tsx** file, send a PUT request to the server based on its argument details. inside the request then function call the `endEdit` method from the argument.

The image illustrates the modified data passed to the `dataSourceChanged` event arguments.

![Next_JS_Edit](../images/next_js_edit.png)

```ts
 // Handle CRUD operations
    async dataSourceChanged(args: DataSourceChangedEventArgs) {
      const response = fetch('/api/health_care', {
        'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ ...args.data, action: 'edit' }),
      });

    if (response.ok) {
      const result = await response.json();
      args?.endEdit?.();
    }
  };

```

**Delete Operation**

Deleting a record from the database involves the following steps:

**Step 1:** Define the "DELETE" method in the **route.ts** file to handle updating an existing product. This method accepts the id from the client and delete the corresponding data from the database.

```ts
// DELETE - Delete a data
export async function DELETE(request: NextRequest) {
    const body = await request.json();
    if (body.action === 'delete') {
        const doctorID = body[0].DoctorId;
        const doctorIndex = doctorDetails.findIndex(u => u.DoctorId === doctorID);
        const deletedDoctor = doctorDetails[doctorIndex];
        doctorDetails.splice(doctorIndex, 1);
        return NextResponse.json({ message: "Doctor deleted successfully", doctor: deletedDoctor });
    }
}
```

**Step 2:** Inside the `dataStateChange` event handler in the **page.tsx** file, send a DELETE request to the server based on its argument details. inside the request then function call the `endEdit` method from the argument.

The image illustrates the deleted data passed to the `dataSourceChanged` event arguments.

![Next_JS_Delete](../images/next_js_delete.png)

```ts
 // Handle CRUD operations
    async dataSourceChanged(args: DataSourceChangedEventArgs) {
      const response = fetch('/api/health_care', {
        'DELETE',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ ...args.data, action: 'delete' }),
      });

    if (response.ok) {
      const result = await response.json();
      args?.endEdit?.();
    }
  };
```

> Normal/Inline editing is the default edit [mode](https://ej2.syncfusion.com/angular/documentation/api/grid/editsettings#mode) for the Grid component. To enable CRUD operations, ensure that the [isPrimaryKey](https://ej2.syncfusion.com/angular/documentation/api/grid/column#isprimarykey) property is set to "true" for a specific Grid Column which has unique values.

## Routing



In this application, routing is used to display the appointment details of doctors. When a user clicks the "View Appointment Details" button present in the each record of the Doctors portal, the selected DoctorID is passed through the router parameters. The patients assigned to that doctor are then displayed on a separate page.

**Step 1:** Create a angular component (**patients**).

**Step 2:** Configure the Doctors portal page with a template column, and enable routing to the corresponding appointment details page when the button inside the template column is clicked.

```ts
[src/app.ts]

export class App {
    constructor(private router: Router) {}
    // Handle appointment button click - navigate to patient page
    btnClick(event: any) {
    const rowData: any = this.gridInstance.getRowInfo(event.target).rowData;
    const doctorID = rowData.DoctorId;
    this.router.navigate(['patients'])
  }
  }
  ```

```html
[src/app.html]
  <ejs-grid #grid [dataSource]="data">
    <e-columns>
       <e-column headerText="Appointments" width="150" [allowEditing]="false">
        <ng-template #template let-data>
          <button ejs-button (click)="btnClick($event)">View Appointments</button>
        </ng-template>
      </e-column>
          {/* Include additional columns here */}
    </e-columns>
  </ejs-grid>
</div>

```

**Step 4:** Add the code for the Grid containing appointment details inside the dynamic route file (**app/Patients/DoctorID/page.tsx**). Retrieve the router parameters and pass the DoctorID to the query property of the Grid. This ensures that only the patients associated with the current doctor are fetched and displayed.

```ts
[src/app.ts]

export class Patients {
    constructor(private route: ActivatedRoute) {}
    ngOnInit(): void {
    // Retrieve the route params
   this.route.queryParams.subscribe((params: any) => {    
      this.doctorID = params['doctorID'];
    });
    // Create query to filter appointments for the specific doctor
    this.query.where('DoctorAssigned', 'equal', this.doctorID, true);
  }

}
}
  ```

```html
[src/app.html]
  <ejs-grid #grid   [dataSource]="data"
  [query]="query">
    <e-columns>
<e-column field="PatientId" headerText="Patient ID" width="120" isPrimaryKey="true"></e-column>
          {/* Include additional columns here */}
    </e-columns>
  </ejs-grid>
</div>

```

## Running the application
- Use the below command to run the application.
 
  ```bash
  ng serve
  ```
- Open http://localhost:4200 in your browser.
 
## Github Sample

For a complete working implementation of this example, refer to the following GitHub repository:
 
[Syncfusion DataGrid with Next.js server Sample](https://github.com/SyncfusionExamples/syncfusion-angular-grid-component-in-nextjs)
 
## Summary
 
This guide demonstrates how to:
1. [Create a Next.js project and install the required packages.](#building-the-nextjs-application)
2. [Configure Next.js route handlers to create server-side API endpoints.](#configuring-nextjs-server)
3. [Integrate Syncfusion angular Grid with the Next.js server using the custom binding feature.](#integrating-the-syncfusion-angular-grid-with-nextjs)
4. [Handle data operations like filtering, searching, sorting, and paging in the Grid.](#performing-data-operation)
5. [Implement CRUD operations (Create, Read, Update, Delete) using POST, GET, PUT, and DELETE methods.](#crud-operations)
6. [Set up navigation to other pages using the Next.js routing feature.](#routing)
7. [Deploy and run the application to manage and display data efficiently in the Grid.](#running-the-application)
 
The application now provides a complete solution for integrating the Syncfusion angular Grid with Next.js server, enabling seamless data operations with a modern, user-friendly interface.