﻿// <auto-generated />
using System;
using ForkAndSpoon.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ForkAndSpoon.Infrastructure.Migrations
{
    [DbContext(typeof(ForkAndSpoonDbContext))]
    [Migration("20250403100702_AddDietaryPreferencesToRecipe")]
    partial class AddDietaryPreferencesToRecipe
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.Favorite", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("RecipeID")
                        .HasColumnType("int");

                    b.HasKey("UserID", "RecipeID");

                    b.HasIndex("RecipeID");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.Ingredient", b =>
                {
                    b.Property<int>("IngredientID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IngredientID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IngredientID");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.Rating", b =>
                {
                    b.Property<int>("RatingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RatingID"));

                    b.Property<int?>("RecipeID")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("RatingID");

                    b.HasIndex("RecipeID");

                    b.HasIndex("UserID");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.Recipe", b =>
                {
                    b.Property<int>("RecipeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RecipeID"));

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedByUserUserID")
                        .HasColumnType("int");

                    b.Property<string>("DietaryPreferences")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Steps")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RecipeID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("CreatedByUserUserID");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.RecipeIngredient", b =>
                {
                    b.Property<int>("RecipeID")
                        .HasColumnType("int");

                    b.Property<int>("IngredientID")
                        .HasColumnType("int");

                    b.HasKey("RecipeID", "IngredientID");

                    b.HasIndex("IngredientID");

                    b.ToTable("RecipeIngredients");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.Favorite", b =>
                {
                    b.HasOne("ForkAndSpoon.Domain.Models.Recipe", "Recipe")
                        .WithMany("Favorites")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ForkAndSpoon.Domain.Models.User", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.Rating", b =>
                {
                    b.HasOne("ForkAndSpoon.Domain.Models.Recipe", "Recipe")
                        .WithMany("Ratings")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ForkAndSpoon.Domain.Models.User", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Recipe");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.Recipe", b =>
                {
                    b.HasOne("ForkAndSpoon.Domain.Models.Category", "Category")
                        .WithMany("Recipes")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ForkAndSpoon.Domain.Models.User", "CreatedByUser")
                        .WithMany("CreatedRecipes")
                        .HasForeignKey("CreatedByUserUserID");

                    b.Navigation("Category");

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.RecipeIngredient", b =>
                {
                    b.HasOne("ForkAndSpoon.Domain.Models.Ingredient", "Ingredient")
                        .WithMany("RecipeIngredients")
                        .HasForeignKey("IngredientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ForkAndSpoon.Domain.Models.Recipe", "Recipe")
                        .WithMany("RecipeIngredients")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.Category", b =>
                {
                    b.Navigation("Recipes");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.Ingredient", b =>
                {
                    b.Navigation("RecipeIngredients");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.Recipe", b =>
                {
                    b.Navigation("Favorites");

                    b.Navigation("Ratings");

                    b.Navigation("RecipeIngredients");
                });

            modelBuilder.Entity("ForkAndSpoon.Domain.Models.User", b =>
                {
                    b.Navigation("CreatedRecipes");

                    b.Navigation("Favorites");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
