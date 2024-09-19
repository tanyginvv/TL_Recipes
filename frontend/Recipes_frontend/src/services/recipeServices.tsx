import { API_URL } from '../constants/apiUrl';
import { IRecipeAllRecipes, IRecipe, IRecipeSubmit, RecipeQueryType, IError } from '../models/types';
import { CheckToken } from '../custom-utils/checkToken';

export class RecipeService {
    private apiUrl: string;

    constructor(apiUrl: string = `${API_URL}/recipes`) {
        this.apiUrl = apiUrl;
    }

    async fetchRecipes(pageNumber: number, searchTerms: string[] = [],
        recipeQueryType: RecipeQueryType)
    : Promise<IRecipeAllRecipes> {
        try {
            const queryString = [
                `pageNumber=${pageNumber}`,
                ...searchTerms.map(term => `searchTerms=${encodeURIComponent(term)}`),
                `recipeQueryType=${recipeQueryType}`,
            ].join('&');
    
            const url = `${this.apiUrl}?${queryString}`;
            
            const headers: HeadersInit = {};

            const token = await CheckToken();
            headers['Access-Token'] = `${token}`;

            const response = await fetch(url, {
                method: 'GET',
                headers: headers,
            });

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
            const token = await CheckToken();
            const headers: HeadersInit = {};
            headers['Access-Token'] = `${token}`;

            const url = `${this.apiUrl}/${id}`;
            const response = await fetch(url, {
                method: 'GET',
                headers: headers,
            });
            
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return await response.json();
        } catch (error) {
            console.error('Error fetching recipe:', error);
            throw error;
        }
    }

    async fetchRecipeOfDay(): Promise<IRecipe> {
        try {
            const response = await fetch(`${this.apiUrl}/recipe-of-day`);
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
            const token = await CheckToken(); 
        
            const headers: HeadersInit = {
                'Access-Token': `${token}`,
            };

            const response = await fetch(`${this.apiUrl}/${id}`, {
                method: 'DELETE',
                headers: headers,
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

    async submitRecipe(recipeData: IRecipeSubmit, id?: string): Promise<IError> {
        try {
            const token = await CheckToken(); 

            const headers: HeadersInit = {
                'Content-Type': 'application/json',
                'Access-Token': `${token}`,
            };
            
            const url = id ? `${this.apiUrl}` : `${this.apiUrl}`;
            const method = id ? 'PUT' : 'POST';

            const response = await fetch(url, {
                method: method,
                headers: headers,
                body: JSON.stringify(recipeData),
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Authentication failed')
            }

            return{};
        } catch (error) {
            return { errorMessage: error}
        }
    }
}
