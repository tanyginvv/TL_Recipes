import { API_URL } from '../constants/apiUrl';
import { CheckToken } from '../custom-utils/checkToken';

export class LikeService {
    private apiUrl: string;

    constructor(apiUrl: string = `${API_URL}/likes`) {
        this.apiUrl = apiUrl;
    }

    async createLike(recipeId: number): Promise<void> {
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
                throw new Error('Ошибка при создании лайка');
            }
        } catch (error) {
            console.error('Error creating like:', error);
            throw error;
        }
    }

    async deleteLike(recipeId: number): Promise<boolean> {
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
                console.error('Failed to delete like:', response.statusText);
                return false;
            }
        } catch (error) {
            console.error('Error deleting like:', error);
            return false;
        }
    }
}
