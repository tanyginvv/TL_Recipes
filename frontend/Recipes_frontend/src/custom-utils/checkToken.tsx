import { AuthenticationService } from "../services/authService";
import { TokenDecoder } from "./tokenDecoder";

export async function CheckToken() {
    const accessToken = localStorage.getItem('AccessToken');

    if (accessToken) {
        const tokenStr = accessToken;
        const decodedToken = TokenDecoder(tokenStr).decoded;
        
        const tokenExpirationDate = new Date(0).setUTCSeconds(decodedToken.exp);
        const now = new Date().getTime();

        if (now >= tokenExpirationDate) {
            const service = new AuthenticationService();
            const newTokens = await service.refreshToken();

            if (newTokens.accessToken && newTokens.refreshToken) {
                await new Promise((resolve) =>{
                    if( newTokens.accessToken)
                    localStorage.setItem('AccessToken', newTokens.accessToken);
                    resolve(true);
                });
            }
            return newTokens.accessToken;
        }
        return accessToken;
    }

    return null;
}