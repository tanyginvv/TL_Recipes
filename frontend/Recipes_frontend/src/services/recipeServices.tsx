import { API_URL } from "../constants/apiUrl";
import { IRecipeAllRecipes } from "../models/types";

export class RecipeService {
    private apiUrl: string;

    constructor(apiUrl: string = API_URL) {
        this.apiUrl = apiUrl;
    }

    async fetchRecipes(pageNumber: number, searchTerms: string[] = []): Promise<IRecipeAllRecipes[]> {
        try {
            const queryString = searchTerms.length > 0
                ? `?${searchTerms.map(term => `searchTerms=${encodeURIComponent(term)}`).join('&')}&pageNumber=${pageNumber}`
                : `?pageNumber=${pageNumber}`;

            const url = `${this.apiUrl}/recipes${queryString}`;

            const response = await fetch(url);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return await response.json();
        } catch (error) {
            console.error('Error fetching recipes:', error);
            throw error;
        }
    }
}
