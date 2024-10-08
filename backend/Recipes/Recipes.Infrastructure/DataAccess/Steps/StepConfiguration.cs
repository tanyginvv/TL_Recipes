﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.DataAccess.Steps;

public class StepConfiguration : IEntityTypeConfiguration<Step>
{
    public void Configure( EntityTypeBuilder<Step> builder )
    {
        builder.ToTable( nameof( Step ) ).HasKey( s => s.Id );

        builder.Property( s => s.Id )
            .IsRequired();

        builder.Property( s => s.StepNumber )
            .IsRequired();

        builder.Property( s => s.StepDescription )
            .IsRequired()
            .HasMaxLength( 250 );

        builder.Property( s => s.RecipeId )
            .IsRequired();
    }
}