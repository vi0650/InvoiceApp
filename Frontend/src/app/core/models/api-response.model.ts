export interface ApiResponse<T> {
  statusCode: number;
  timestamp: string;
  value: {
    data: T;
    errors: any;
    message: string;
    success: boolean;
    statusCode: number;
    timestamp: string;
  };
}