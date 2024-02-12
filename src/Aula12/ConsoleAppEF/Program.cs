using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
var optionsBuilder = new DbContextOptionsBuilder<YourDbContext>();
optionsBuilder.UseInMemoryDatabase("Data Source=Exemplo.db");

using (var context = new YourDbContext(optionsBuilder.Options))
{
    context.Database.EnsureCreated();
    // CREATE
    var newStudent = new Student { Name = "John Doe" };
    context.Students.Add(newStudent);
    context.SaveChanges();
    Console.WriteLine("Novo estudante criado com ID: " + newStudent.StudentId);

    // READ
    Console.WriteLine("\nTodos os estudantes:");
    ListAllStudents(context);

    // UPDATE
    var studentToUpdate = context.Students.FirstOrDefault();
    if (studentToUpdate != null)
    {
        studentToUpdate.Name = "Jane Smith";
        context.SaveChanges();
        Console.WriteLine("\nEstudante atualizado com sucesso.");
        ListAllStudents(context);
    }

    // DELETE
    var studentToDelete = context.Students.FirstOrDefault(s => s.Name == "John Doe");
    if (studentToDelete != null)
    {
        context.Students.Remove(studentToDelete);
        context.SaveChanges();
        Console.WriteLine("\nEstudante excluído com sucesso.");
        ListAllStudents(context);
    }
}

void ListAllStudents(YourDbContext context)
{
    var students = context.Students.ToList();

    foreach (var student in students)
    {
        Console.WriteLine($"Aluno {student.Name}");
    }
}

public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }

    // Shadow property for many-to-many relationship
    public ICollection<Course> Courses { get; set; }
}

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; }

    // Shadow property for many-to-many relationship
    public ICollection<Student> Students { get; set; }
}

public class YourDbContext : DbContext
{
    public YourDbContext(DbContextOptions<YourDbContext> options) : base(options) { }

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Courses)
            .WithMany(c => c.Students);
            //.UsingEntity(j => j.ToTable("StudentCourse"));
    }
}

