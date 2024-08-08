import { ITag } from "../models/types";
export class TagService {
    private apiUrl: string;

    constructor(apiUrl: string) {
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
