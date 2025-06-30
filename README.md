# BlazeBuy


## The project
A simple .NET 9, Blazor SSR web application. Utilizing Entity Framework, Clean Architecture, Radzen Components, Stripe (payments) and external login options such as Microsoft Login

It features basic role-based access control of pages, basic product and category creation, order management capabilities for admins, integration with stripe for credit card payments, external login and account creation

Admins can create coupons, such coupons may have a fixed amount or be a percentage of the value, being that a product's value or the entire order's value. The coupons will be created on both a local database and Stripe so that if the payment method is changed there would still be a registry of past coupons and to possibly make future integrations easier

**Observation:** Screenshots taken with "GoFullPage" which may cause inconsistencies for example in the admin navMenu not extending all the way down to the bottom of the page and backgroud due to stitching multiple screenshots

# Images
## Unauthenticated user pages:
Home Page:
![image](https://github.com/user-attachments/assets/2896f24e-3905-4f80-bc87-96a53fa7aa65)

Sign In page:
![image](https://github.com/user-attachments/assets/d5214f2f-2f78-451d-bf71-0fc5945856cf)

Sign Up page:
![image](https://github.com/user-attachments/assets/36b65ff4-c2dc-4800-b773-4062e234c68f)

----
## Customer pages
Home page:
![image](https://github.com/user-attachments/assets/444ae1db-59ad-48b9-b483-7b2595cd0af1)

Shopping Cart:
![image](https://github.com/user-attachments/assets/557df6c4-0474-4644-8273-f7d3bc4d0d22)

Order List:
![image](https://github.com/user-attachments/assets/51df3687-e905-4709-b3f9-5972f17dbb42)

Order Details:
![image](https://github.com/user-attachments/assets/38ffcd7d-6717-45a2-88d7-5ddb728bdabb)

----
## Admin pages
Home page:
![image](https://github.com/user-attachments/assets/fe00ba34-d558-4ae0-8eac-2172523afc8a)

Order List:
![image](https://github.com/user-attachments/assets/98518c32-17c0-4933-a508-6dc435349003)

Order Details:
![image](https://github.com/user-attachments/assets/fec28af0-4b93-4baa-866c-b97ce2d75907)

Category List:
![image](https://github.com/user-attachments/assets/7578c4fb-8854-4b85-a7a9-b023b0bfef42)

Create Category:
![image](https://github.com/user-attachments/assets/2b75e2f9-5f0e-43eb-96f5-ec58091d25fc)

Product List:
![image](https://github.com/user-attachments/assets/1e74f2aa-3832-446a-afaf-2572758d4846)

Create Product:
![image](https://github.com/user-attachments/assets/5d1cc463-e237-4128-a83c-8e4fa82455e8)

Coupon List:
![image](https://github.com/user-attachments/assets/e90b8046-7239-469f-a7eb-fc136ebda3ae)

Create Coupon:
![image](https://github.com/user-attachments/assets/25199f80-7598-4fee-a2d9-4e47cf045de1)

Shopping Cart:
![image](https://github.com/user-attachments/assets/c7f926cb-bec5-4d8e-9e8f-9f43f4cb463a)

