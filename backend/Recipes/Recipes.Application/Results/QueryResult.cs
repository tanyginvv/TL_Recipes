﻿using Recipes.Application.Validation;

namespace Recipes.Application.Results
{
    public class QueryResult<TQueryResultData> where TQueryResultData : class
    {
        public ValidationResult ValidationResult { get; private set; }
        public TQueryResultData ObjResult { get; private set; }

        public QueryResult( TQueryResultData objResult )
        {
            ObjResult = objResult;
            ValidationResult = ValidationResult.Ok();
        }

        public QueryResult( ValidationResult validationResult )
        {
            ValidationResult = validationResult;
        }
    }
}