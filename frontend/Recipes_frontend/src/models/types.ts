export interface IRecipe {
    id: number,
    authorLogin: string,
    authorId: number,
    name: string,
    description: string,
    portionCount: number,
    likeCount: number,
    isLike: boolean,
    favouriteCount: number;
    isFavourite: boolean,
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
    authorLogin: string,
    name: string,
    description: string,
    portionCount: number,
    likeCount: number,
    favouriteCount: number;
    cookTime: number,
    imageUrl: string,
    tags: ITag[],
    isLike?: boolean,
    isFavourite?: boolean
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