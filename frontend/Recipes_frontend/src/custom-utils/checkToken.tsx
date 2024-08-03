import { AuthenticationService } from "../services/authService";
import { TokenDecoder } from "./tokenDecoder";

export async function CheckToken() {
    if (localStorage.getItem('AccessToken') != null) {
        const tokenStr: string = "" + localStorage.getItem('AccessToken');
        if (new Date() >= new Date(new Date(0).setSeconds(TokenDecoder(tokenStr).decoded.exp))) {
            const service: AuthenticationService = new AuthenticationService();
            await service.refreshToken(); 
        }
    }
}