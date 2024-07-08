using Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Recipes.Domain.Entities;

namespace Recipes.API.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipeController( IRecipeRepository recipeRepository )
        {
            _recipeRepository = recipeRepository;
        }

        // GET: api/recipe
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetAllRecipes()
        {
            var recipes = await _recipeRepository.GetAllAsync();
            return Ok( recipes );
        }

        // GET: api/recipe/{id}
        [HttpGet( "{id}" )]
        public async Task<ActionResult<Recipe>> GetRecipeById( int id )
        {
            var recipe = await _recipeRepository.GetByIdAsync( id );

            if ( recipe == null )
            {
                return NotFound();
            }

            return Ok( recipe );
        }

        // POST: api/recipe
        [HttpPost]
        public async Task<ActionResult<Recipe>> CreateRecipe( Recipe recipe )
        {
            await _recipeRepository.AddAsync( recipe );
            return CreatedAtAction( nameof( GetRecipeById ), new { id = recipe.Id }, recipe );
        }

        // PUT: api/recipe/{id}
        [HttpPut( "{id}" )]
        public async Task<IActionResult> UpdateRecipe( int id, Recipe recipe )
        {
            if ( id != recipe.Id )
            {
                return BadRequest();
            }

            var existingRecipe = await _recipeRepository.GetByIdAsync( id );
            if ( existingRecipe == null )
            {
                return NotFound();
            }

            existingRecipe.SetName( recipe.Name );
            existingRecipe.SetDescription( recipe.Description );
            existingRecipe.SetCookTime( recipe.CookTime );
            existingRecipe.SetCountPortion( recipe.CountPortion );
            existingRecipe.SetImageUrl( recipe.ImageUrl );

            await _recipeRepository.UpdateAsync( existingRecipe );

            return NoContent();
        }

        // DELETE: api/recipe/{id}
        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteRecipe( int id )
        {
            var existingRecipe = await _recipeRepository.GetByIdAsync( id );
            if ( existingRecipe == null )
            {
                return NotFound();
            }

            await _recipeRepository.DeleteAsync( id );

            return NoContent();
        }
    }
}
