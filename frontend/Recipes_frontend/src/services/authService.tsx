import { IToken } from "../models/types";
import { IAuthentication, IRegister } from "../models/types";
import { Cookies } from "react-cookie";
import { API_URL } from "../constants/apiUrl";

export class AuthenticationService {
    private apiUrl: string;

    constructor(apiUrl: string = `${API_URL}/users`) {
        this.apiUrl = apiUrl;
    }

    async register(body: IRegister): Promise<IToken> {
        try {
            const response = await fetch(`${this.apiUrl}/register`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(body),
            });
    
            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message);
            }
    
            const loginData: IAuthentication = {
                Login: body.Login,
                Password: body.Password
            };
    
            const authResponse = await this.authentication(loginData);
    
            return authResponse;
        } catch (error) {
            return {
                accessToken: null,
                refreshToken: null,
                errorMessage: error
            };
        }
    }

    async refreshToken(): Promise<IToken> {
        try {
            const cookie = new Cookies();
            const refreshToken = await cookie.get("RefreshToken");
    
            if (!refreshToken) {
                throw new Error('No refresh token available');
            }
    
            const response = await fetch(`${this.apiUrl}/refresh-token`, {
                method: "POST",
                credentials: "include",
            });
    
            if (!response.ok) {
                throw new Error('Failed to refresh token');
            }
    
            const data: IToken = await response.json();
            
            if (data.accessToken && data.refreshToken) {
                localStorage.removeItem('AccessToken');
                cookie.remove('RefreshToken');
                localStorage.setItem('AccessToken', data.accessToken);
                cookie.set('RefreshToken', data.refreshToken, { path: '/' })
            }
            
            return data;
        } catch (error) {
            this.logout();
            return {
                accessToken: null,
                refreshToken: null,
            };
        }
    }

    async authentication(body: IAuthentication): Promise<IToken> {
        try {
            const cookies = new Cookies();
            const response = await fetch(`${this.apiUrl}/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(body),
            });
    
            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Authentication failed');
            }
    
            const data: IToken = await response.json();
    
            localStorage.removeItem('AccessToken');
            cookies.remove('RefreshToken');
    
            if (data.accessToken) {
                localStorage.setItem('AccessToken', data.accessToken);
            }
    
            if (data.refreshToken) {
                cookies.set('RefreshToken', data.refreshToken, { path: '/' });
            }
    
            window.location.reload();
            return data;
        } catch (error) {
            console.error('Error during authentication:', error);
            return {
                accessToken: null,
                refreshToken: null,
                errorMessage: error
            };
        }
    }

    logout(): void {
        const cookie = new Cookies();
        localStorage.removeItem("AccessToken");
        cookie.remove("RefreshToken");
    }
}
