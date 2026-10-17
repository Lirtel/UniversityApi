using Microsoft.EntityFrameworkCore;
using UniversityApi.Entities;

namespace UniversityApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Fluent API — уникальные индексы
        modelBuilder.Entity<Student>().HasIndex(s => s.Email).IsUnique();
        modelBuilder.Entity<Teacher>().HasIndex(t => t.Email).IsUnique();

        // Один студент не может дважды записаться на один курс
        modelBuilder.Entity<Enrollment>()
            .HasIndex(e => new { e.StudentId, e.CourseId })
            .IsUnique();

        // Связи и FK
        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Teacher)
            .WithMany(t => t.Courses)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        // Начальные данные
        modelBuilder.Entity<Teacher>().HasData(
            new Teacher { Id = 1, FirstName = "Ivan", LastName = "Petrov", Email = "ivan@uni.kz", Department = "CS" },
            new Teacher { Id = 2, FirstName = "Anna", LastName = "Sidorova", Email = "anna@uni.kz", Department = "Math" }
        );

        modelBuilder.Entity<Course>().HasData(
            new Course { Id = 1, Name = "Web Development", Description = "ASP.NET Core", Credits = 5, TeacherId = 1, CreatedAt = new DateTime(2024, 1, 1) },
            new Course { Id = 2, Name = "Databases", Description = "SQL & EF Core", Credits = 4, TeacherId = 1, CreatedAt = new DateTime(2024, 1, 1) }
        );
    }
}