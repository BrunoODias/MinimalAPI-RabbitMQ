import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef, provideZoneChangeDetection  } from '@angular/core';
import { ProductModel } from '../../models/product';
import { ProductService } from '../../services/product'
import { signal } from '@angular/core';



@Component({
  selector: 'app-product-list',
  imports: [CommonModule],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css',
})
export class ProductList implements OnInit {
  constructor(private _service: ProductService) { }
  
  products = signal<ProductModel[]>([]);
  name: string = '';
  price: number = 0;

  ngOnInit(): void {
    this._service.getProducts(this.name, this.price).subscribe(
      x => {
        this.products.set(x);
      }
    )
  }
}
