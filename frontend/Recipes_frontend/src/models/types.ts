export interface IRecipe {
    id: number,
    name: string,
    description: string,
    countPortion: number,
    cookTime: number,
    tags: ITag[],
    ingredients: IIngredient[],
    steps: IStep[]
}

interface ITag {
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