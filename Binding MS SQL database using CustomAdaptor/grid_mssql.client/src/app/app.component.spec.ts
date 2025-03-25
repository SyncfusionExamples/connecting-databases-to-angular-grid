import { UrlAdaptor } from '@syncfusion/ej2-data';

export class CustomAdaptor extends UrlAdaptor {
  public override processResponse(): any {
    // Calling base class processResponse function.
    const original: any = super.processResponse.apply(this, arguments as any);
    return original;
  }

  public override insert(dm: any, data: any): any {
    return {
      url: dm.dataSource.insertUrl,
      data: JSON.stringify({
        value: data,
        action: 'insert'
      }),
      type: 'POST',
      headers: {
        'Authorization': 'Bearer your_token',
        'Content-Type': 'application/json'
      }
    };
  }

  public override update(dm: any, value: any): any {
    return {
      url: dm.dataSource.updateUrl,
      data: JSON.stringify({
        value: value,
        action: 'update'
      }),
      type: 'POST',
      headers: {
        'Authorization': 'Bearer your_token',
        'Content-Type': 'application/json'
      }
    };
  }

  public override remove(dm: any, keyField: string, value: any): any {
    return {
      url: dm.dataSource.removeUrl,
      data: JSON.stringify({
        key: value,
        keyColumn: keyField,
        action: 'remove'
      }),
      type: 'POST',
      headers: {
        'Authorization': 'Bearer your_token',
        'Content-Type': 'application/json'
      }
    };
  }
}
