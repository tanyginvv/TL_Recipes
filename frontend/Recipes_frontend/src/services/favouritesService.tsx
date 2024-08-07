import { API_URL } from '../constants/apiUrl';
import { IFavouriteBool } from '../models/types';
import { CheckToken } from '../custom-utils/checkToken';

export class FavouriteService {
    private apiUrl: string;

    constructor(apiUrl: string = API_URL) {
        this.apiUrl = apiUrl;
    }

    async createFavourite(recipeId: number, userId: number): Promise<void> {
        try {
            const token = await CheckToken(); 

            const headers: HeadersInit = {
                'Content-Type': 'application/json',
                'Access-Token': `${token}`,
            };

            const url = `${this.apiUrl}/favourites/${userId}/${recipeId}`;

            const response = await fetch(url, {
                method: 'POST',
                headers: headers,
            });

            if (!response.ok) {
                throw new Error('Ошибка при добавлении в избранное');
            }
        } catch (error) {
            console.error('Error creating favourite:', error);
            throw error;
        }
    }

    async deleteFavourite(recipeId: number, userId: number): Promise<boolean> {
        try {
            const token = await CheckToken(); 

            const headers: HeadersInit = {
                'Access-Token': `${token}`,
            };

            const url = `${this.apiUrl}/favourites/${userId}/${recipeId}`;

            const response = await fetch(url, {
                method: 'DELETE',
                headers: headers,
            });

            if (response.ok) {
                return true;
            } else {
                console.error('Failed to delete favourite:', response.statusText);
                return false;
            }
        } catch (error) {
            console.error('Error deleting favourite:', error);
            return false;
        }
    }

    async getFavouritesCount(recipeId: number): Promise<number> {
        try {
            const response = await fetch(`${this.apiUrl}/favourites/count/${recipeId}`);

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            return data.count.count;
        } catch (error) {
            console.error('Error fetching favourites count:', error);
            throw error;
        }
    }

    async getFavouriteStatus(userId: number, recipeId: number): Promise<IFavouriteBool> {
        try {
            const response = await fetch(`${this.apiUrl}/favourites/${userId}/${recipeId}`);

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
             
            const data = await response.json();
            return data;
        } catch (error) {
            console.error('Error fetching favourite status:', error);
            throw error;
        }
    }
}
