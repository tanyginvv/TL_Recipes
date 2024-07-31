import create from 'zustand';
import { Cookies } from 'react-cookie';
import {jwtDecode} from 'jwt-decode';
import { IDecryptedToken } from '../models/types';
import { AuthenticationService } from '../services/authService';

interface AuthState {
    accessToken: string | null;
    refreshToken: string | null;
    userId: number | null;
    isLoggedIn: boolean;
    setUserId: (userId: number | null) => void;
    setAccessToken: (token: string | null) => void;
    setRefreshToken: (token: string | null) => void;
    setIsLoggedIn: (isLoggedIn: boolean) => void;
    logout: () => void;
    checkToken: () => Promise<void>; 
}

interface PopupState {
    isRegistrationWindowOpen: boolean;
    isAuthorizationWindowOpen: boolean;
    isAuthOrRegistrWindowOpen: boolean;
    setRegistrationWindowOpen: (isOpen: boolean) => void;
    setAuthorizationWindowOpen: (isOpen: boolean) => void;
    setAuthOrRegistrWindowOpen: (isOpen: boolean) => void;
}

const TokenDecoder = (token: string): { decoded: IDecryptedToken } => {
    const decoded : IDecryptedToken = jwtDecode<IDecryptedToken>(token);
    return {decoded};
};

// Store implementation using Zustand
const useStore = create<AuthState & PopupState>((set) => {
    const cookies = new Cookies();
    const accessToken = localStorage.getItem("AccessToken") || null;
    const refreshToken = cookies.get("RefreshToken") || null;
    const userId = accessToken ? TokenDecoder(accessToken).decoded.userId : null;
    const isLoggedIn = !!accessToken;

    const updateTokens = (token: string | null) => {
        if (token) {
            localStorage.setItem("AccessToken", token);
            set({ 
                accessToken: token, 
                userId: TokenDecoder(token).decoded.userId,
                isLoggedIn: true 
            });
        } else {
            localStorage.removeItem("AccessToken");
            set({ 
                accessToken: null, 
                userId: null,
                isLoggedIn: false 
            });
        }
    };

    const updateRefreshToken = (token: string | null) => {
        if (token) {
            cookies.set("RefreshToken", token);
        } else {
            cookies.remove("RefreshToken");
        }
        set({ refreshToken: token });
    };

    const setUserId = (userId: number | null) => {
        set({ userId });
    };

    const setIsLoggedIn = (isLoggedIn: boolean) => {
        set({ isLoggedIn });
    };

    const logout = () => {
        localStorage.removeItem("AccessToken");
        cookies.remove("RefreshToken");
        set({ accessToken: null, refreshToken: null, userId: null, isLoggedIn: false });
    };

    const checkToken = async () => {
        if (accessToken) {
            const decodedToken = TokenDecoder(accessToken);
            const expiryTime = decodedToken.decoded.exp * 1000;
            const currentTime = new Date().getTime();

            if (currentTime >= expiryTime) {
                try {
                    const service = new AuthenticationService();
                    const newTokens = await service.refreshToken();

                    if (newTokens.accessToken && newTokens.refreshToken) {
                        updateTokens(newTokens.accessToken);
                        updateRefreshToken(newTokens.refreshToken);
                        setIsLoggedIn(true);
                    } else {
                        logout(); 
                    }
                } catch (error) {
                    console.error("Token refresh error:", error);
                    logout(); 
                }
            }
        } else {
            setIsLoggedIn(false);
        }
    };

    return {
        accessToken,
        refreshToken,
        userId,
        isLoggedIn,
        setAccessToken: updateTokens,
        setRefreshToken: updateRefreshToken,
        setUserId,
        setIsLoggedIn,
        logout,
        checkToken,

        isRegistrationWindowOpen: false,
        isAuthorizationWindowOpen: false,
        isAuthOrRegistrWindowOpen: false,
        setRegistrationWindowOpen: (isOpen) => set({ isRegistrationWindowOpen: isOpen }),
        setAuthorizationWindowOpen: (isOpen) => set({ isAuthorizationWindowOpen: isOpen }),
        setAuthOrRegistrWindowOpen: (isOpen) => set({ isAuthOrRegistrWindowOpen: isOpen }),
    };
});

export default useStore;
