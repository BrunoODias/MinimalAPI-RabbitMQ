import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private api =
    'https://localhost:7022/product';

  constructor(
    private http: HttpClient
  ) {}

  private getProductsUrl(name: string, price: number): string {
    return `${this.api}/products?productName=${name}&productPrice=${price}`;
  }

  getProducts(name: string, price: number) {
    return this.http.get<any[]>(this.getProductsUrl(name, price), );
  }
}