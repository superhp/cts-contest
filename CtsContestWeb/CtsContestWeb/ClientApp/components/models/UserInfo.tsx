export interface UserInfo {
    isLoggedIn: boolean;
    email: string;
    name: string;
    totalBalance: number;
    totalWins: number;
    totalLooses: number;
    picture?: string;
}