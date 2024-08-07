import { API_URL } from '../constants/apiUrl';
import { ILikeBool } from '../models/types';
import { CheckToken } from '../custom-utils/checkToken';

export class LikeService {
    private apiUrl: string;

    constructor(apiUrl: string = API_URL) {
        this.apiUrl = apiUrl;
    }

    async createLike(recipeId: number, userId: number ): Promise<void> {
        try {
            const token = await CheckToken(); 

            const headers: HeadersInit = {
                'Content-Type': 'application/json',
                'Access-Token': `${token}`,
            };

            const url = `${this.apiUrl}/likes/${userId}/${recipeId}`;

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

    async deleteLike(recipeId: number, userId: number): Promise<boolean> {
        try {
            const token = await CheckToken(); 

            const headers: HeadersInit = {
                'Access-Token': `${token}`,
            };

            const url = `${this.apiUrl}/likes/${userId}/${recipeId}`;

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

    async getLikesCount(recipeId: number): Promise<number> {
        try {
            const response = await fetch(`${this.apiUrl}/likes/count/${recipeId}`);

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            return data.count.count;
        } catch (error) {
            console.error('Error fetching likes count:', error);
            throw error;
        }
    }

    async getLikeStatus(userId: number, recipeId: number): Promise<ILikeBool> {
        try {
            const response = await fetch(`${this.apiUrl}/likes/${userId}/${recipeId}`);

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            
            const data = await response.json();

            return data;
        } catch (error) {
            console.error('Error fetching like status:', error);
            throw error;
        }
    }
}
