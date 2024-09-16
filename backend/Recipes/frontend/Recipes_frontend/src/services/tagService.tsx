import { API_URL } from '../constants/apiUrl';
import { ITag } from '../models/types';

export class TagService {
    private apiUrl: string;

    constructor(apiUrl: string = `${API_URL}/tags`) {
        this.apiUrl = apiUrl;
    }

    async fetchTags(): Promise<ITag[]> {
        try {
            const response = await fetch(this.apiUrl);
            if (response.ok) {
                return await response.json();
            } else {
                console.error('Failed to fetch tags:', response.statusText);
                return [];
            }
        } catch (error) {
            console.error('Error fetching tags:', error);
            return [];
        }
    }
}
