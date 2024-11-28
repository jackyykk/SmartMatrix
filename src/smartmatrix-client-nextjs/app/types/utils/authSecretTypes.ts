export interface AuthSecret {
    loginName: string;
    userName: string;
    token: AuthToken;
}

export interface AuthToken {
    accessToken: string;
    accessToken_LifeInMinutes: number;
    accessToken_Expires: string;
    refreshToken: string;
    refreshToken_LifeInMinutes: number;
    refreshToken_Expires: string;
}