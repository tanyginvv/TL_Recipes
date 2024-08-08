using Mapster;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Tags.Commands.GetOrCreateTag;
using Recipes.Application.UseCases.Tags.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags
{
    public class StepMappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<GetOrCreateTagCommand, Tag>
                .NewConfig()
                .ConstructUsing( src => new Tag( src.Name ) );

            TypeAdapterConfig<TagDto, GetOrCreateTagCommand>
                .NewConfig();

            TypeAdapterConfig<Tag, GetTagByNameQueryDto>
                .NewConfig();

            TypeAdapterConfig<Tag, TagDto>
                .NewConfig();
        }
    }
}