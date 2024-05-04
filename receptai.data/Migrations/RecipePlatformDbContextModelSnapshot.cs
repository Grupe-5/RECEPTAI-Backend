﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using receptai.data;

#nullable disable

namespace receptai.data.Migrations
{
    [DbContext(typeof(RecipePlatformDbContext))]
    partial class RecipePlatformDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("SubfoodditUser", b =>
                {
                    b.Property<int>("SubfoodditsSubfoodditId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsersUserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SubfoodditsSubfoodditId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("SubfoodditUser");
                });

            modelBuilder.Entity("receptai.data.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CommentDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<int>("RecipeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CommentId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("receptai.data.CommentVote", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER")
                        .HasColumnOrder(0);

                    b.Property<int>("CommentId")
                        .HasColumnType("INTEGER")
                        .HasColumnOrder(1);

                    b.Property<DateTime>("VoteDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("VoteType")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "CommentId");

                    b.HasIndex("CommentId");

                    b.ToTable("CommentVotes");
                });

            modelBuilder.Entity("receptai.data.Image", b =>
                {
                    b.Property<int>("ImgId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("ImgId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("receptai.data.Recipe", b =>
                {
                    b.Property<int>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CookingDifficulty")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CookingTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(5000)
                        .HasColumnType("TEXT");

                    b.Property<int?>("ImgId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ingredients")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Instructions")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Servings")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SubfoodditId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RecipeId");

                    b.HasIndex("ImgId");

                    b.HasIndex("SubfoodditId");

                    b.HasIndex("UserId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("receptai.data.RecipeVote", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER")
                        .HasColumnOrder(0);

                    b.Property<int>("RecipeId")
                        .HasColumnType("INTEGER")
                        .HasColumnOrder(1);

                    b.Property<DateTime>("VoteDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("VoteType")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "RecipeId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeVotes");
                });

            modelBuilder.Entity("receptai.data.Subfooddit", b =>
                {
                    b.Property<int>("SubfoodditId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("SubfoodditId");

                    b.ToTable("Subfooddits");
                });

            modelBuilder.Entity("receptai.data.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ImgId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("KarmaScore")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.HasIndex("ImgId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SubfoodditUser", b =>
                {
                    b.HasOne("receptai.data.Subfooddit", null)
                        .WithMany()
                        .HasForeignKey("SubfoodditsSubfoodditId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("receptai.data.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("receptai.data.Comment", b =>
                {
                    b.HasOne("receptai.data.Recipe", "Recipe")
                        .WithMany("Comments")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("receptai.data.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("User");
                });

            modelBuilder.Entity("receptai.data.CommentVote", b =>
                {
                    b.HasOne("receptai.data.Comment", "Comment")
                        .WithMany("Votes")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("receptai.data.User", "User")
                        .WithMany("CommentVotes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("receptai.data.Recipe", b =>
                {
                    b.HasOne("receptai.data.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImgId");

                    b.HasOne("receptai.data.Subfooddit", "Subfooddit")
                        .WithMany("Recipes")
                        .HasForeignKey("SubfoodditId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("receptai.data.User", "User")
                        .WithMany("Recipes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("Subfooddit");

                    b.Navigation("User");
                });

            modelBuilder.Entity("receptai.data.RecipeVote", b =>
                {
                    b.HasOne("receptai.data.Recipe", "Recipe")
                        .WithMany("Votes")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("receptai.data.User", "User")
                        .WithMany("RecipeVotes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("User");
                });

            modelBuilder.Entity("receptai.data.User", b =>
                {
                    b.HasOne("receptai.data.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImgId");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("receptai.data.Comment", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("receptai.data.Recipe", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("receptai.data.Subfooddit", b =>
                {
                    b.Navigation("Recipes");
                });

            modelBuilder.Entity("receptai.data.User", b =>
                {
                    b.Navigation("CommentVotes");

                    b.Navigation("Comments");

                    b.Navigation("RecipeVotes");

                    b.Navigation("Recipes");
                });
#pragma warning restore 612, 618
        }
    }
}
