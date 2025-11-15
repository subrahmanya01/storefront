import { Component, OnInit, ViewChild } from '@angular/core';
import { BreadScrumComponent } from '../../building-blocks/bread-scrum/bread-scrum.component';
import { ButtonComponent } from '../../building-blocks/button/button.component';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CreateOrderRequest, OrderPriceRequest, OrderPriceResponse, ProductDiscount, ProductInfomation, ShippingAddress } from '../../interfaces/order';
import { CartService } from '../../services/cart.service';
import { CartItem, CartItemResponse, CartResponse } from '../../interfaces/cart-item';
import { ExtendedProduct, ProductsByIdsRequest } from '../../interfaces/product-meta';
import { ProductInfoService } from '../../services/product-info.service';
import { OrderService } from '../../services/order.service';
import { DiscountsService } from '../../services/discounts.service';
import { ValidateCouponRequest, ValidateCouponResponse } from '../../interfaces/discounts';
import { Router } from '@angular/router';
import { SpinnerComponent } from '../../building-blocks/spinner/spinner.component';
import { ShippingChargeService } from '../../services/shipping-charge.service';

@Component({
  selector: 'app-checkout',
  imports: [
    BreadScrumComponent, 
    ButtonComponent,
    ReactiveFormsModule,
    CommonModule,
    SpinnerComponent
  ],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent implements OnInit {
  checkoutForm!: FormGroup;
  cartInfo: CartResponse | null = null;
  mappedCartItems: CartItem[] = [];
  totalCartAmount: number = 0;
  isCouponCodeApplied: boolean = false;
  isInvalidCoupon: boolean = false;
  validCouponCode: string = "";
  isLoading: boolean = false;
  isSubmitted: boolean = false;
  postalCodes: string[] = [];
  orderAmount: OrderPriceResponse = {} as OrderPriceResponse;

  @ViewChild('checkoutFormRef') checkoutFormRef: any;

  constructor(private fb: FormBuilder, 
    private readonly cartService: CartService, 
    private readonly orderService: OrderService,
    private readonly discountService: DiscountsService,
    private readonly shippingService: ShippingChargeService,
    private readonly router: Router,
    private readonly productInfoService:ProductInfoService) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.getCartProducts();
    this.getPostalCodes();
    this.checkoutForm = this.fb.group({
      firstName: ['', Validators.required],
      companyName: [''],
      streetAddress: ['', Validators.required],
      apartment: [''],
      postalCode:['', [Validators.required, this.allowedPostalCodesValidator(this.postalCodes)]],
      townCity: ['', Validators.required],
      country: ['', Validators.required],
      state: ['', Validators.required],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      emailAddress: ['', [Validators.required, Validators.email]]
    });

    this.checkoutForm.get('postalCode')!.statusChanges.subscribe(status => {
      if (status === 'VALID') {
        this.getOrderAmount();
      }
    });
  }

  allowedPostalCodesValidator(allowedPostalCodes: string[]): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const value = control.value;
      if (!value || this.postalCodes.includes(value)) {

        return null; 
      }
      console.log("not allowe")

      return { invalidPostalCode: true }; 
    };
  }
  
  getShippingAddress(): ShippingAddress {
    const formValue = this.checkoutForm.value;

    const address: ShippingAddress = {
      fullName: formValue.firstName,
      line1: formValue.companyName,
      line2: formValue.streetAddress || null,
      city: formValue.townCity,
      state: formValue.state || null,
      postalCode: formValue.postalCode,
      country: formValue.country,
      phoneNumber: formValue.phoneNumber || null,
      email: formValue.emailAddress || null
    };

    return address;
  }
  
  getCartProducts()
  {
    this.cartService.cartInfo$.subscribe({
      next: (data: CartResponse | null) => {
        this.cartInfo = data;
        if(!data || data.items.length == 0)
        {
          this.isLoading = false;
          return;
        }
        const productIds = data?.items.map((x:CartItemResponse)=> x.productId);
        if(!productIds || productIds?.length == 0)
        {
          this.isLoading = false;
          return;
        }
        const request = {productIds: productIds} as ProductsByIdsRequest;
        this.productInfoService.getProductsByIds(request).subscribe({
          next: (products: ExtendedProduct[])=>{
            this.createCartItemModel(data.items, products)
          }
        })
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  getPostalCodes()
  {
    this.shippingService.getPostalCodes().subscribe({
      next: (data) => {
        this.postalCodes = data.filter(x=> x.length > 0);
        console.log("postal", data);
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  getOrderAmount()
  {
    const postalCode = this.checkoutForm.get("postalCode")?.value;
    const request = {
      productInfomations: this.cartInfo?.items.map((item:CartItemResponse)=>{
        return {
          productId: item.productId,
          variantId: item.variantId,
          quantity: item.quantity
        } as ProductInfomation
      }),
      subTotal: this.totalCartAmount,
      couponCode: this.validCouponCode,
      postalCode: postalCode.length == 0? null : postalCode
    } as OrderPriceRequest;
    this.orderService.getOrderPriceInfo(request).subscribe({
      next: (data) => {
        this.orderAmount = data;
        console.log(this.orderAmount);
      },
      error: (err) => {
        console.error(err.message);
      }
    });
  }

  createCartItemModel(cartItems: CartItemResponse[], products: ExtendedProduct[])
  {
    const newCartItems: CartItem[] = []
    for(let item of cartItems)
    {
      const product = products.find((x:ExtendedProduct)=> x.id == item.productId);
      if(product)
      {
        const variantInfo = product.variants?.find(x=> x.id == item.variantId);
        if(variantInfo)
        {
          newCartItems.push({
            cartId: item.id,
            name: product.name,
            price: variantInfo.price,
            quantity: item.quantity,
            imageUrl: variantInfo.images[0],
            variantId: variantInfo.id,
            productId: product.id,
            productData: product
          } as CartItem)
        }
      }
    }
    this.mappedCartItems = newCartItems;
    this.totalCartAmount = newCartItems.map(x=> x.price* x.quantity).reduce((acc, curr) => acc + curr, 0);
    this.isLoading = false;
  }

  onSubmit(): void {
    this.isSubmitted = true;
    if (this.checkoutForm.valid) {
      const shippingAddress = this.getShippingAddress();
      const request = {
        cartId: this.cartInfo?.id,
        shippingAddress: shippingAddress,
        couponCode: this.validCouponCode
      } as CreateOrderRequest;
      this.orderService.placeOrder(request).subscribe({
        next: (data) => {
          this.cartService.clearCart().subscribe();
          this.router.navigateByUrl("/my-orders");
          this.isSubmitted = false;
        },
        error: (err) => {
          this.isSubmitted = false;
          console.error(err.message);
        }
      });
      console.log(this.checkoutForm.value);
    } else {
      this.checkoutForm.markAllAsTouched(); 
    }
  }

  get f() {
    return this.checkoutForm.controls;
  }

  applyCoupon(value: string)
  {
    const requst = {
      code: value,
      orderAmount: this.totalCartAmount,
      productId: null,
      category: null
    } as ValidateCouponRequest;

    this.discountService.validateCoupon(requst).subscribe({
      next: (data: ValidateCouponResponse) => {
        if(data.isValid)
        {
          this.validCouponCode = value;
          this.isCouponCodeApplied = true;
          this.isInvalidCoupon = false;
          this.getOrderAmount();
        }
        else
        {
          this.isCouponCodeApplied = false;
          this.isInvalidCoupon = true;
          this.validCouponCode = "";
          this.getOrderAmount();
        }
      },
      error: (err) => {
        this.isCouponCodeApplied = false;
        this.isInvalidCoupon = true;
        this.validCouponCode = "";
        console.error(err.message);
        this.getOrderAmount();
        
      }
    });
  }

  handleCouponClear(value: string)
  {
    if(value.trim().length == 0)
    {
      this.applyCoupon(value);
    }
  }

  triggerSubmit(): void {
    this.checkoutFormRef.onSubmit(); 
  }

  getDiscountForProduct(productId: string, variantId: string)
  {
    return this.orderAmount.discounts?.filter((x: ProductDiscount)=> x.productId == productId && x.variantId == variantId)?.[0]?.discount;
  }

  getDiscountedPrice(productId: string, variantId: string, price: number)
  {
    const discount = this.getDiscountForProduct(productId, variantId);
    if(discount)
    {
      return price - price * (discount/ 100);
    }
    return price;
  }

  getSubTotalAfterDiscount()
  {
    let subTotal = 0;
    for(let item of this.cartInfo?.items??[])
    {
      subTotal+= this.getDiscountedPrice(item.productId, item.variantId, item.price);
    }
    return subTotal;
  }
}
