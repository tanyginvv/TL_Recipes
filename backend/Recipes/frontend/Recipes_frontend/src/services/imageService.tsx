import { API_URL } from "../constants/apiUrl";
import { CheckToken } from "../custom-utils/checkToken";
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

    async uploadImage(image: File): Promise<string> {
        const token = await CheckToken(); 

        const formData = new FormData();
        formData.append('image', image);
    
        const headers: HeadersInit = {
            'Access-Token': `${token}`,
        };
    
        const response = await fetch(`${this.apiUrl}/images/upload`, {
            method: 'POST',
            headers: headers,
            body: formData,
        });
    
        if (!response.ok) {
            throw new Error('Ошибка при загрузке изображения');
        }
    
        const data = await response.json();
        return data.fileName;
    }
}