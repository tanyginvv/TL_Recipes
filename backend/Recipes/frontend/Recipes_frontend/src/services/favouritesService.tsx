import { API_URL } from '../constants/apiUrl';
import { CheckToken } from '../custom-utils/checkToken';

export class FavouriteService {
    private apiUrl: string;

    constructor(apiUrl: string = `${API_URL}/favourites`) {
        this.apiUrl = apiUrl;
    }

    async createFavourite(recipeId: number): Promise<void> {
        try {
            const token = await CheckToken(); 

            const headers: HeadersInit = {
                'Content-Type': 'application/json',
                'Access-Token': `${token}`,
            };

            const url = `${this.apiUrl}/${recipeId}`;

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

    async deleteFavourite(recipeId: number): Promise<boolean> {
        try {
            const token = await CheckToken(); 

            const headers: HeadersInit = {
                'Access-Token': `${token}`,
            };

            const url = `${this.apiUrl}/${recipeId}`;

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
}