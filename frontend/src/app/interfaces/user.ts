export interface UserResponse {
    id: string;
    createdAt: string;
    modifiedAt: string;
    firstName: string | null;
    lastName: string | null;
    email: string | null;
    phoneNumber: string | null;
}

export interface UserUpdateRequest {
    firstName: string ;
    lastName: string | null ;
    email: string ;
    phoneNumber: string | null;
    currentPassword: string | null;
    newPassword: string | null;
}