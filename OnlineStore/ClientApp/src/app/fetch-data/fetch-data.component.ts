import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public orders: Orders[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router: Router) {
    http.get<Orders[]>(baseUrl + 'api/Orders').subscribe(result => {
      this.orders = result;
    }, error => console.error(error));
  }

  public loadDetails(orderId: number) {
    this.router.navigateByUrl('/order-details?Id=' + orderId);
  }
}

interface Orders {
  orderId: number;
  customerName: string;
  orderDate: string;
}
