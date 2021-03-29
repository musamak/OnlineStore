import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html'
})
export class OrderDetailsComponent {
  public order: Order;
  public baseUrl: string;
  public http: HttpClient;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    var url = new URL(window.location.href);
    var orderId = url.searchParams.get("Id");

    this.baseUrl = baseUrl;
    this.http = http;

    http.get<Order>(baseUrl + 'api/Orders/' + orderId).subscribe(result => {
      this.order = result;
    }, error => console.error(error));
  }

  public updateDetails(orderId: number, trackingNumber: string) {
    var Url = this.baseUrl + 'api/Orders/' + orderId + '/' + trackingNumber;
    this.http.put<any>(Url, null).subscribe(result => {
    document.getElementById('trackingNumber').innerHTML = result.d;
    }, error => console.error(error));
  }
}

 interface Product {
  productId: number;
  name: string;
  price: number;
}

 interface OrderDetail {
  orderDetailId: number;
  orderId: number;
  productId: number;
  product: Product;
  quantity: number;
  price: number;
}

 interface ShippingAddress {
  addressId: number;
  street1: string;
  street2: string;
  city: string;
  state: string;
  zip: string;
}

 interface BillingAddress {
  addressId: number;
  street1: string;
  street2: string;
  city: string;
  state: string;
  zip: string;
}

 interface Customer {
  customerId: number;
  firstName: string;
  lastName: string;
  email: string;
  shippingAddressId: number;
  shippingAddress: ShippingAddress;
  billingAddressId: number;
  billingAddress: BillingAddress;
}

 interface Order {
  orderId: number;
  customerId: number;
  orderDate: Date;
  shippingPrice: number;
  trackingNumber: string;
  orderDetails: OrderDetail[];
  customer: Customer;
}
