import { API_URL } from '../constants/apiUrl';
import { IRecipeAllRecipes, IRecipe, IRecipeSubmit } from '../models/types';

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

    async fetchRecipeById(id: string): Promise<IRecipe> {
        try {
            const response = await fetch(`${this.apiUrl}/recipes/${id}`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return await response.json();
        } catch (error) {
            console.error('Error fetching recipe:', error);
            throw error;
        }
    }

    async deleteRecipe(id: number): Promise<boolean> {
        try {
            const response = await fetch(`${this.apiUrl}/recipes/${id}`, {
                method: 'DELETE',
            });
            if (response.ok) {
                return true;
            } else {
                console.error('Failed to delete recipe:', response.statusText);
                return false;
            }
        } catch (error) {
            console.error('Error deleting recipe:', error);
            return false;
        }
    }

    async submitRecipe(recipeData: IRecipeSubmit, id?: string): Promise<void> {
        const url = id ? `${this.apiUrl}/recipes/${id}` : `${this.apiUrl}/recipes`;
        const method = id ? 'PUT' : 'POST';

        const response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(recipeData),
        });

        if (!response.ok) {
            throw new Error('Ошибка при обработке рецепта');
        }
    }
}