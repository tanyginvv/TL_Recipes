export interface IRecipe {
    id: number,
    name: string,
    description: string,
    countPortion: number,
    cookTime: number,
    tags: ITag[],
    ingredients: IIngredient[],
    steps: Step[]
}

interface ITag {
    name: string
}

interface IIngredient {
    title: string,
    description: string
}

interface Step {
    stepNumber: number,
    description: string
}