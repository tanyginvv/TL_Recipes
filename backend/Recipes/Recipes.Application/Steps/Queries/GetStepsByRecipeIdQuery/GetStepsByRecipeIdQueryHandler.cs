﻿using Application.CQRSInterfaces;
using Application.Result;
using Application.Validation;
using Recipes.Application.Steps.Dtos;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Repositories;

namespace Recipes.Application.Steps.Queries
{
    public class GetStepsByRecipeIdQueryHandler : IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery>
    {
        private readonly IStepRepository _stepRepository;
        private readonly IAsyncValidator<GetStepsByRecipeIdQuery> _stepQueryValidator;

        public GetStepsByRecipeIdQueryHandler(
            IStepRepository stepRepository,
            IAsyncValidator<GetStepsByRecipeIdQuery> validator )
        {
            _stepRepository = stepRepository;
            _stepQueryValidator = validator;
        }

        public async Task<QueryResult<GetStepsByRecipeIdQueryDto>> HandleAsync( GetStepsByRecipeIdQuery query )
        {
            ValidationResult validationResult = await _stepQueryValidator.ValidationAsync( query );
            if ( validationResult.IsFail )
            {
                return new QueryResult<GetStepsByRecipeIdQueryDto>( validationResult );
            }

            IReadOnlyList<Step> steps = await _stepRepository.GetByRecipeIdAsync( query.RecipeId );
            if ( steps == null )
            {
                return new QueryResult<GetStepsByRecipeIdQueryDto>( ValidationResult.Fail( "Steps not found" ) );
            }

            var dto = new GetStepsByRecipeIdQueryDto
            {
                RecipeId = query.RecipeId,
                Steps = new List<Step>( steps )
            };

            return new QueryResult<GetStepsByRecipeIdQueryDto>( dto );
        }
    }
}
