import { API_URL } from "../constants/apiUrl";

export class ImageService {
    private apiUrl: string;

    constructor(apiUrl: string = API_URL) {
        this.apiUrl = apiUrl;
    }

    async fetchImage(imageUrl: string): Promise<string | null> {
        try {
            const response = await fetch(`${this.apiUrl}/images/${imageUrl}`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const imageBlob = await response.blob();
            return URL.createObjectURL(imageBlob);
        } catch (error) {
            console.error('Ошибка при загрузке изображения:', error);
            return null;
        }
    }
}
