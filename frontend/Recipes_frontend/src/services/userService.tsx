import { API_URL } from '../constants/apiUrl';
import {  IError, IName, IUser, IUserUpdate } from '../models/types';
import { CheckToken } from '../custom-utils/checkToken';

export class UserService {
    private apiUrl: string;

    constructor(apiUrl: string = `${API_URL}/users` ) {
        this.apiUrl = apiUrl;
    }

    async fetchUserName(): Promise<IName> {
        try {
            const token = await CheckToken();

            const headers: HeadersInit = {
                'Access-Token': `${token}`,
                'Content-Type': 'application/json',
            };

            const response = await fetch(`${this.apiUrl}/name`, {
                method: 'GET',
                headers: headers
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error('Error fetching user:', error);
            throw error;
        }
    }

    async fetchUser(): Promise<IUser> {
        try {
            const token = await CheckToken();

            const headers: HeadersInit = {
                'Access-Token': `${token}`
            };

            const response = await fetch(`${this.apiUrl}/current-user`, {
                method: 'GET',
                headers: headers
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error('Error fetching user:', error);
            throw error;
        }
    }

    async updateUser(userData: IUserUpdate): Promise<IError> {
        try {
            const token = await CheckToken();
        
            const headers: HeadersInit = {
                'Access-Token': `${token}`,
                'Content-Type': 'application/json',
            };

            const response = await fetch(`${this.apiUrl}/current-user`, {
                method: 'PUT',
                headers: headers,
                body: JSON.stringify(userData),
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Authentication failed')
            }
            return {};
        } catch (error) {
            return { errorMessage: error}
        }
    }
}