export interface CompleteRegistrationRequest {
    uniqueId: string;
}

export interface ForgotPasswordRequest {
    email: string;
}

export interface ResetPasswordRequest {
    newPassword: string;
}