import { API_URL } from '../constants/apiUrl';
import {  IError, ILogin, IRecipeAllRecipes, IUser, IUserUpdate } from '../models/types';
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

    async updateUser(id: number, userData: IUserUpdate): Promise<IError> {
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
                const errorData = await response.json();
                throw new Error(errorData.message || 'Authentication failed')
            }
            return {};
        } catch (error) {
            return { errorMessage: error}
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

    async fetchUserRecipes(userId: number, pageNumber: number = 1, searchTerms: string[] = []): Promise<IRecipeAllRecipes[]> {
        try {
            const token = await CheckToken();

            const headers: HeadersInit = {
                'Access-Token': `${token}`,
                'Content-Type': 'application/json',
            };
            const query = new URLSearchParams();
            query.append('pageNumber', pageNumber.toString());
            searchTerms.forEach(term => query.append('searchTerms', term));

            const response = await fetch(`${this.apiUrl}/users/${userId}/recipes?${query.toString()}`, {
                method: 'GET',
                headers: headers
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error('Error fetching user recipes:', error);
            throw error;
        }
    }
}
