export interface StoreInfo {
    name: string | null;
    address: string | null;
    website: string | null;
    companySupportEmails: string[] | null;
    phoneNumber: string[] | null;
    tagLine: string | null;
    mainEmail: string | null;
    socialMediaLinks: SocialMediaLinks|null;
}

export interface SocialMediaLinks {
    facebook: string | null;
    linkedIn: string | null;
    twitter: string | null;
    instagram: string | null;
}