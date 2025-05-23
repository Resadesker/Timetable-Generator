﻿// <auto-generated />
using System;
using Backend_Generator.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend_Generator.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.28");

            modelBuilder.Entity("Backend_Generator.Model.LessonAssignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClassId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SubjectId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeacherId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("RoomId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("Backend_Generator.Model.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AvailabilityJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RoomType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Backend_Generator.Model.SchoolClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("Backend_Generator.Model.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RequiredRoomType")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TeacherId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("Backend_Generator.Model.SubjectRequirement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SchoolClassId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SubjectId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WeeklyHours")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SchoolClassId");

                    b.HasIndex("SubjectId");

                    b.ToTable("SubjectRequirement");
                });

            modelBuilder.Entity("Backend_Generator.Model.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AvailabilityJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("Backend_Generator.Model.LessonAssignment", b =>
                {
                    b.HasOne("Backend_Generator.Model.SchoolClass", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend_Generator.Model.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend_Generator.Model.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend_Generator.Model.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Backend_Generator.Model.TimeSlot", "TimeSlot", b1 =>
                        {
                            b1.Property<int>("LessonAssignmentId")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Day")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Period")
                                .HasColumnType("INTEGER");

                            b1.HasKey("LessonAssignmentId");

                            b1.ToTable("Lessons");

                            b1.WithOwner()
                                .HasForeignKey("LessonAssignmentId");
                        });

                    b.Navigation("Class");

                    b.Navigation("Room");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");

                    b.Navigation("TimeSlot")
                        .IsRequired();
                });

            modelBuilder.Entity("Backend_Generator.Model.Subject", b =>
                {
                    b.HasOne("Backend_Generator.Model.Teacher", null)
                        .WithMany("CanTeachSubjects")
                        .HasForeignKey("TeacherId");
                });

            modelBuilder.Entity("Backend_Generator.Model.SubjectRequirement", b =>
                {
                    b.HasOne("Backend_Generator.Model.SchoolClass", null)
                        .WithMany("SubjectRequirements")
                        .HasForeignKey("SchoolClassId");

                    b.HasOne("Backend_Generator.Model.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Backend_Generator.Model.SchoolClass", b =>
                {
                    b.Navigation("SubjectRequirements");
                });

            modelBuilder.Entity("Backend_Generator.Model.Teacher", b =>
                {
                    b.Navigation("CanTeachSubjects");
                });
#pragma warning restore 612, 618
        }
    }
}
