export interface IRecipe {
    id: number,
    authorLogin: string,
    authorId: number,
    name: string,
    description: string,
    portionCount: number,
    likeCount: number,
    isLiked: boolean,
    favouriteCount: number;
    isFavourited: boolean,
    cookTime: number,
    imageUrl: string,
    tags: ITag[],
    ingredients: IIngredient[],
    steps: IStep[]
}

export interface IRecipeSubmit {
    name: string,
    userId: number | null,
    description: string,
    portionCount: number,
    cookTime: number,
    imageUrl: string,
    tags: ITag[],
    ingredients: IIngredient[],
    steps: IStep[]
}

export interface IRecipeAllRecipes {
    getRecipePartDtos: IRecipePart[],
    isNextPageAvailable: boolean
}

export interface IRecipePart {
    id: number,
    authorLogin: string,
    name: string,
    description: string,
    portionCount: number,
    likeCount: number,
    favouriteCount: number;
    cookTime: number,
    imageUrl: string,
    tags: ITag[],
    isLiked?: boolean,
    isFavourited?: boolean
}
export interface ITag {
    name: string
}

export interface IIngredient {
    title: string,
    description: string
}

export interface IStep {
    stepNumber: number,
    stepDescription: string
}

export interface IAuthentication {
    Login: string,
    Password: string
}

export interface IRegister {
    Name: string,
    Login: string,
    Password: string,
    Description: string
}

export interface IToken {
    accessToken: string | null,
    refreshToken: string | null,
    errorMessage?: unknown
}

export interface IDecryptedToken{
    userId: number,
    exp: number
}

export interface IUser {
    id: number,
    name: string,
    description: string,
    login: string,
    recipeCount: number,
    favouriteCount: number,
    likeCount: number
}

export interface ILogin {
    id: number,
    login: string
}

export interface IName {
    name: string
}

export interface IUserUpdate {
   name?: string,
   description?: string,
   login?: string,
   oldPassword?: string,
   newPassword?: string,
   error?: unknown
}

export interface IError {
    status?: number;
    errorMessage?: unknown
}

export enum RecipeQueryType {
    All,
    My, 
    Starred
}