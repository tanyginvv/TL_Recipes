export interface IRecipe {
    id: number,
    userId: number,
    name: string,
    description: string,
    portionCount: number,
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
    id: number,
    userId: number,
    name: string,
    description: string,
    portionCount: number,
    cookTime: number,
    imageUrl: string,
    tags: ITag[]
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
    PasswordHash: string
}

export interface IRegister {
    Name: string,
    Login: string,
    PasswordHash: string,
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
    recipesCount: number,
    favouritesCount: number,
    likesCount: number
}

export interface ILogin {
    id: number,
    login: string
}

export interface IUserUpdate {
   name: string,
   description: string,
   login: string,
   oldPasswordHash: string,
   newPasswordHash: string,
   error?: unknown
}

export interface IError {
    status?: number;
    errorMessage?: unknown
}

export interface ILikeBool {
    value: {
        isLiked: boolean
    } 
}

export interface IFavouriteBool {
    value: {
        isFavourite: boolean
    } 
}