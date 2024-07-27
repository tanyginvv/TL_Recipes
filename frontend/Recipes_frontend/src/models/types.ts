export interface IRecipe {
    id: number,
    name: string,
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