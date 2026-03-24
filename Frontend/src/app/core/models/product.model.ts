export interface Product {
    productId: string;
    productName: string;
    rate: number | null | any;
    isActive: boolean;
    createdAt: Date;
    updatedAt: Date;
}

// export interface ProductValue {
//     data: Product[];
//     errors: any;
//     message: string;
//     statusCode: number;
//     success: boolean;
//     timestamp: string;
// }

// export interface ProductResponse {
//     statusCode: number;
//     timestamp: string;
//     value: ProductValue;
// }