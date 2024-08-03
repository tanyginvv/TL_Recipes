import { API_URL } from '../constants/apiUrl';
import {  ILogin, IUser, IUserUpdate } from '../models/types';

export class UserService {
    private apiUrl: string;

    constructor(apiUrl: string = API_URL ) {
        this.apiUrl = apiUrl;
    }

    private getHeaders(): HeadersInit {
        const headers: HeadersInit = {
            'Content-Type': 'application/json'
        };

        return headers;
    }

    async fetchUser(id: number): Promise<IUser> {
        try {
            const response = await fetch(`${this.apiUrl}/Users/${id}`, {
                method: 'GET',
                headers: this.getHeaders()
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
                method: 'GET',
                headers: this.getHeaders()
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
            const response = await fetch(`${this.apiUrl}/users/${id}`, {
                method: 'PUT',
                headers: this.getHeaders(),
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
            const response = await fetch(`${this.apiUrl}/users/${id}?passwordHash=${encodeURIComponent(passwordHash)}`, {
                method: 'DELETE',
                headers: this.getHeaders()
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
