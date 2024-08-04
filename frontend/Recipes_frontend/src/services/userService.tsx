import { API_URL } from '../constants/apiUrl';
import {  ILogin, IUser, IUserUpdate } from '../models/types';
import { CheckToken } from '../custom-utils/checkToken';

export class UserService {
    private apiUrl: string;

    constructor(apiUrl: string = API_URL ) {
        this.apiUrl = apiUrl;
    }

    async fetchUser(id: number): Promise<IUser> {
        try {
            const response = await fetch(`${this.apiUrl}/Users/${id}`, {
                method: 'GET',
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

    async fetchUserLogin(id: number): Promise<ILogin> {
        try {
            const response = await fetch(`${this.apiUrl}/Users/login/${id}`, {
                method: 'GET'
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
    async updateUser(id: number, userData: IUserUpdate): Promise<void> {
        try {
            const token = await CheckToken();
        
            const headers: HeadersInit = {
                'Access-Token': `${token}`,
                'Content-Type': 'application/json',
            };

            const response = await fetch(`${this.apiUrl}/users/${id}`, {
                method: 'PUT',
                headers: headers,
                body: JSON.stringify(userData),
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
        } catch (error) {
            console.error('Error updating user:', error);
            throw error;
        }
    }

    async deleteUser(id: number, passwordHash: string): Promise<void> {
        try {
            const token = await CheckToken();
        
            const headers: HeadersInit = {
                'Access-Token': `${token}`,
            };
            const response = await fetch(`${this.apiUrl}/users/${id}?passwordHash=${encodeURIComponent(passwordHash)}`, {
                method: 'DELETE',
                headers: headers
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
        } catch (error) {
            console.error('Error deleting user:', error);
            throw error;
        }
    }
}
