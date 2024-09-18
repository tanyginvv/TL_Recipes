import create from 'zustand';
import { Cookies } from 'react-cookie';
import { TokenDecoder } from '../custom-utils/tokenDecoder';
import { IUser } from '../models/types'; 

interface AuthState {
    accessToken: string | null;
    refreshToken: string | null;
    userId: number | null;
    isLoggedIn: boolean;
    user: IUser | null;
    setUserId: (userId: number | null) => void;
    setAccessToken: (token: string | null) => void;
    setIsLoggedIn: (isLoggedIn: boolean) => void;
    setUser: (user: IUser | null) => void; 
    logout: () => void;
    notificationText: string;
    notification: boolean;
    notificationStatus: string;
    setNotification: (notificationText: string, notificationStatus: string) => void;
    setCloseNotification: () => void;
}

interface PopupState {
    isRegistrationWindowOpen: boolean;
    isAuthorizationWindowOpen: boolean;
    isAuthOrRegistrWindowOpen: boolean;
    setRegistrationWindowOpen: (isOpen: boolean) => void;
    setAuthorizationWindowOpen: (isOpen: boolean) => void;
    setAuthOrRegistrWindowOpen: (isOpen: boolean) => void;
}

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

    const setUserId = (userId: number | null) => set({ userId });
    const setIsLoggedIn = (isLoggedIn: boolean) => set({ isLoggedIn });
    const setUser = (user: IUser | null) => set({ user });

    const logout = () => {
        localStorage.removeItem("AccessToken");
        cookies.remove("RefreshToken");
        set({ accessToken: null, refreshToken: null, userId: null, isLoggedIn: false, user: null });
    };

    const setNotification = (notificationText: string, notificationStatus: string) => {
        set({ notificationText, notification: true, notificationStatus });
        setTimeout(() => {
            set({ notification: false });
        }, 3000);
    };

    const setCloseNotification = () => {
        set({ notification: false });
    };

    const checkTokens = () => {
        const currentAccessToken = localStorage.getItem("AccessToken");
        const currentRefreshToken = cookies.get("RefreshToken");

        if (!currentAccessToken || !currentRefreshToken) {
            logout();
        }

        set({ accessToken: currentAccessToken, refreshToken: currentRefreshToken });
    };

    setInterval(checkTokens, 1000);

    return {
        accessToken,
        refreshToken,
        userId,
        isLoggedIn,
        user: null,
        setAccessToken: updateTokens,
        setUserId,
        setIsLoggedIn,
        setUser, 
        logout,
        notification: false,
        notificationText: '',
        notificationStatus: '',
        setNotification,
        setCloseNotification,

        isRegistrationWindowOpen: false,
        isAuthorizationWindowOpen: false,
        isAuthOrRegistrWindowOpen: false,
        setRegistrationWindowOpen: (isOpen) => set({ isRegistrationWindowOpen: isOpen }),
        setAuthorizationWindowOpen: (isOpen) => set({ isAuthorizationWindowOpen: isOpen }),
        setAuthOrRegistrWindowOpen: (isOpen) => set({ isAuthOrRegistrWindowOpen: isOpen }),
    };
});

export default useStore;
