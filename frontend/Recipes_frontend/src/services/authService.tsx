import { IToken } from "../models/types";
import { IAuthentication, IRegister } from "../models/types";
import { Cookies } from "react-cookie";
import { API_URL } from "../constants/apiUrl";
import useStore from "../store/store";

export class AuthenticationService {
    private apiUrl: string;

    constructor(apiUrl: string = `${API_URL}/users`) {
        this.apiUrl = apiUrl;
    }

    async register(body: IRegister): Promise<IToken> {
        try {
            const response = await fetch(`${this.apiUrl}/registrate`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(body),
            });
    
            if (!response.ok) {
                throw new Error('Registration failed');
            }
    
            const loginData: IAuthentication = {
                Login: body.Login,
                PasswordHash: body.PasswordHash
            };
    
            const authResponse = await this.authentication(loginData);
            
            if (authResponse.accessToken && authResponse.refreshToken) {
                const { setAccessToken, setRefreshToken, setIsLoggedIn } = useStore.getState();
                setAccessToken(authResponse.accessToken);
                setRefreshToken(authResponse.refreshToken);
                setIsLoggedIn(true);
            }
    
            return authResponse;
        } catch (error) {
            console.error('Registration Error:', error);
            return {
                accessToken: null,
                refreshToken: null
            };
        }
    }

    async refreshToken(): Promise<IToken> {
        try {
            const cookie = new Cookies();
            const refreshToken = cookie.get("RefreshToken");
    
            if (!refreshToken) {
                throw new Error('No refresh token available');
            }
    
            const response = await fetch(`${this.apiUrl}/refresh-token`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `${refreshToken}`,
                },
                credentials: "include",
            });
    
            if (!response.ok) {
                throw new Error('Failed to refresh token');
            }
    
            const data: IToken = await response.json();
    
            if (data.accessToken && data.refreshToken) {
                const { setAccessToken, setRefreshToken } = useStore.getState();
                await setAccessToken(data.accessToken);
                await setRefreshToken(data.refreshToken);
            }
            
            return data;
        } catch (error) {
            console.error('Token refresh error:', error);
            return {
                accessToken: null,
                refreshToken: null,
            };
        }
    }    

    async authentication(body: IAuthentication): Promise<IToken> {
        try {
            const cookie = new Cookies()
            const response = await fetch(`${this.apiUrl}/authentication`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(body),
            });

            if (!response.ok) {
                throw new Error('Authentication failed');
            }

            const data: IToken = await response.json();

            localStorage.removeItem("AccessToken");
            cookie.remove("RefreshToken");
            
            localStorage.setItem("AccessToken", JSON.stringify(data.accessToken));
            cookie.set("RefreshToken", data.refreshToken);

            return data;
        } catch (Error) {
            return {
                accessToken: null,
                refreshToken: null
            };
        }
    }

    logout(): void {
        const cookie = new Cookies();
        localStorage.removeItem("AccessToken");
        cookie.remove("RefreshToken");
    }
}
