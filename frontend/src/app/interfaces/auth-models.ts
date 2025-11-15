export interface LoginRequest {
    email: string;
    phoneNumber: string;
    password: string;
}

export interface RegisterRequest
{
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    password: string;
}

export interface RefreshTokenRequest
{
    userId: string;
    refreshToken: string;
}

export interface AuthResponse
{
    success: boolean;
    message: string;
    token: string;
    tokenExpiresAt: string;
    refreshToken: string;
}