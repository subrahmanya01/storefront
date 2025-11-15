import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { IndexPageComponent } from './pages/index-page/index-page.component';
import { CartComponent } from './pages/cart/cart.component';
import { PaymentsComponent } from './pages/payments/payments.component';
import { UserProfileComponent } from './pages/user-profile/user-profile.component';
import { ProductInfoComponent } from './pages/product-info/product-info.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { ContactComponent } from './pages/contact/contact.component';
import { CheckoutComponent } from './pages/checkout/checkout.component';
import { AboutComponent } from './pages/about/about.component';
import { HomeComponent } from './admin-pages/home/home.component';
import { ManageProductComponent } from './admin-pages/manage-product/manage-product.component';
import { authGuard } from './guards/auth.guard';
import { deactivateLoginGuard } from './guards/deactivate-login.guard';
import { SearchResultComponent } from './pages/search-result/search-result.component';
import { ManageAppComponent } from './admin-pages/manage-app/manage-app.component';
import { BannerImagesComponent } from './admin-pages/components/banner-images/banner-images.component';
import { FlashSalesComponent } from './admin-pages/components/flash-sales/flash-sales.component';
import { WatchListComponent } from './pages/watch-list/watch-list.component';
import { MyOrdersComponent } from './pages/my-orders/my-orders.component';
import { ManageOrdersComponent } from './admin-pages/manage-orders/manage-orders.component';
import { deactivateAdminGuard } from './guards/deactivate-admin-guard';
import { VerifyEmailComponent } from './pages/verify-email/verify-email.component';
import { ForgotPasswordComponent } from './pages/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './pages/reset-password/reset-password.component';
import { OrderInfoComponent } from './admin-pages/order-info/order-info.component';
import { TaxComponent } from './admin-pages/tax/tax.component';
import { ManageShippingChargeComponent } from './admin-pages/manage-shipping-charge/manage-shipping-charge.component';
import { ManageDiscountsComponent } from './admin-pages/manage-discounts/manage-discounts.component';
import { MyReviewsComponent } from './pages/my-reviews/my-reviews.component';

export const routes: Routes = [
    {
        path:'',
        component: IndexPageComponent,
        data: { breadcrumb: 'Home' }
    },
    {
        path:'login',
        component: LoginComponent,
        canActivate: [deactivateLoginGuard]
    },
    {
        path:'register',
        component: RegisterComponent,
        canActivate: [deactivateLoginGuard]
    },
    {
        path:'verify-email',
        component: VerifyEmailComponent,
        canActivate: [deactivateLoginGuard]
    },
    {
        path:'verify-email/:id',
        component: VerifyEmailComponent,
        canActivate: [deactivateLoginGuard]
    },
    {
        path:'forgot-password',
        component: ForgotPasswordComponent,
        canActivate: [deactivateLoginGuard]
    },
    {
        path:'reset-password/:id',
        component: ResetPasswordComponent,
        canActivate: [deactivateLoginGuard]
    },
    {
        path:'cart',
        component: CartComponent,
        canActivate: [authGuard],
        data: { breadcrumb: 'Cart' }
    },
    {
        path:'payment',
        component: PaymentsComponent,
        canActivate: [authGuard]
    },
    {
        path:'profile',
        component: UserProfileComponent,
        canActivate: [authGuard],
        data: { breadcrumb: 'Profile' }
    },
    {
        path:'product/:id',
        component: ProductInfoComponent,
        data: { breadcrumb: 'Product' }
    },
    {
        path:'contact',
        component: ContactComponent,
        data: { breadcrumb: 'Contact' }
    },
    {
        path:'my-orders',
        component: MyOrdersComponent,
        canActivate: [authGuard],
        data: { breadcrumb: 'My Orders' }
    },
    {
        path:'watchlist',
        component: WatchListComponent,
        canActivate: [authGuard],
        data: { breadcrumb: 'WatchList' }
    },
    {
        path:'checkout',
        component: CheckoutComponent,
        canActivate: [authGuard],
        data: { breadcrumb: 'Checkout' }
    },
    {
        path:'about',
        component: AboutComponent,
        data: { breadcrumb: 'About' }

    },
    {
        path:'search',
        component: SearchResultComponent,
        data: { breadcrumb: 'Search' }
    },
    {
        path:'my-reviews',
        component: MyReviewsComponent,
        data: { breadcrumb: 'My Reviews' }
    },
    {
        path:'admin',
        component: HomeComponent,
        canActivate: [authGuard, deactivateAdminGuard],
        data: { breadcrumb: 'Admin' }
    },
    { 
        path: 'admin/product/new',
        component: ManageProductComponent,
        canActivate: [authGuard, deactivateAdminGuard],
        data: { breadcrumb: 'New Product' }
    },
    { 
        path: 'admin/product/:id',
        component: ManageProductComponent ,
        canActivate: [authGuard, deactivateAdminGuard]
    },
    { 
        path: 'admin/manage',
        component: ManageAppComponent ,
        canActivate: [authGuard, deactivateAdminGuard],
        children:[
            {
                path:"",
                component: BannerImagesComponent
            },
            {
                path:"banner",
                component: BannerImagesComponent
            },
            {
                path:"flash-sales",
                component: FlashSalesComponent
            },
            {
                path:"orders",
                component: ManageOrdersComponent,
            },
            {
                path:"order/:id",
                component: OrderInfoComponent,
            },
            {
                path:"tax",
                component: TaxComponent,
            },
            {
                path:"shipping-charge",
                component: ManageShippingChargeComponent,
            },
            {
                path:"discounts",
                component: ManageDiscountsComponent,
            }
        ]
    },
    {
        path:'**',
        component: NotFoundComponent
    },
];
