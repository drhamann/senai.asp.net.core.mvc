# Aula 12 

- EF

# Materiais 

    
    - https://learn.microsoft.com/pt-br/dotnet/csharp/linq/
	- https://learn.microsoft.com/pt-br/ef/
	- https://learn.microsoft.com/pt-br/ef/core/
    - https://learn.microsoft.com/pt-br/ef/core/modeling/relationships/many-to-many
    - https://learn.microsoft.com/pt-br/ef/core/saving/cascade-delete

### Nuget


```
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design

// Code first
dotnet ef migrations add InitialCreate
dotnet ef database update 

```

Gerar contexto de banco já criado
```
//Ex: Use o console após ter instalado as ferramentas
//Install-Package Microsoft.EntityFrameworkCore.SqlServer
//Install-Package Microsoft.EntityFrameworkCore.Tools
Scaffold-DbContext "Server=NomeDoServidor;Database=NomeDoBancoDeDados;User Id=NomeDeUsuario;Password=SuaSenha;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context MeuContexto

```

```
// Injeção de dependencia do contexto 

 services.AddDbContext<ApplicationDbContext>(
        options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"))

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}	

```

```
//Criando o contexto direto no dbcontext
public class ApplicationDbContext : DbContext
{
    private readonly string _connectionString;

    public ApplicationDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
}
```

```
//Defenindo o modelo

... DbContext
 protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.Entity<Blog>()
            .Property(b => b.Url)
            .IsRequired();
    }

    // Classe de configuração usada em conjunto com definição do assembly
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(u => u.Role).IsRequired();
            builder.Property(u => u.UserName).IsRequired();
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.Password).IsRequired();
        }
    }

    //context.Database.Migrate(); // Apply migrations


    // Transaction

    using (var context = new YourDbContext(optionsBuilder.Options))
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                ...
                    context.SaveChanges();

                    // Commit transaction
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Handle exception
                    Console.WriteLine("An error occurred: " + ex.Message);
                    // Rollback transaction
                    transaction.Rollback();
                }
            }
        }

    // 1 -n
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasMany(e => e.Posts)
        .WithOne(e => e.Blog)
        .HasForeignKey(e => e.BlogId)
        .IsRequired();

    modelBuilder.Entity<Post>()
        .HasOne(e => e.Blog)
        .WithMany(e => e.Posts)
        .HasForeignKey(e => e.BlogId)
        .IsRequired();
}

// 1-1

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasOne(e => e.Header)
        .WithOne(e => e.Blog)
        .HasForeignKey<BlogHeader>(e => e.BlogId)
        .IsRequired();

    modelBuilder.Entity<BlogHeader>()
        .HasOne(e => e.Blog)
        .WithOne(e => e.Header)
        .HasForeignKey<BlogHeader>(e => e.BlogId)
        .IsRequired();
}

//n -n 
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
            .WithMany(c => c.Students)
            .UsingEntity(j => j.ToTable("StudentCourse"));
    }
}

// Configuração para remover  

 /*modelBuilder
        .Entity<Blog>()
        .HasOne(e => e.Owner)
        .WithOne(e => e.OwnedBlog)
        .OnDelete(DeleteBehavior.ClientCascade)
*/
```

Crud básico
```
var optionsBuilder = new DbContextOptionsBuilder<YourDbContext>();
        optionsBuilder.UseSqlServer("YourConnectionString");

        using (var context = new YourDbContext(optionsBuilder.Options))
        {
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
```

Padrão repository

```
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> GetByIdAsync(int id);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet.AsQueryable();
    }

    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
    }
}


public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; }
}

public class StudentRepository : Repository<Student>, IStudentRepository
{
    public StudentRepository(YourDbContext context) : base(context)
    {
        await context.Database.EnsureCreatedAsync();

        // Seed some initial data when the StudentRepository is constructed
        SeedDataAsync().Wait(); // You can also make Main method async and await it
    }

    private async Task SeedDataAsync()
    {
        // Check if any students exist
        if (await GetAll().AnyAsync())
        {
            return; // Data has already been seeded
        }

        // Seed some students
        await AddAsync(new Student { Name = "John Doe" });
        await AddAsync(new Student { Name = "Jane Smith" });

        // Save changes
        await SaveChangesAsync();
    }
}


public class YourDbContext : DbContext
{
    public YourDbContext(DbContextOptions<YourDbContext> options) : base(options) { }
    
}

class Program
{
    static async Task Main(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<YourDbContext>();
        optionsBuilder.UseSqlite("Data Source=students.db"); // SQLite connection string


        using (var context = new YourDbContext(optionsBuilder.Options))
        {
            var studentRepository = new StudentRepository(context);

            // Create
            var newStudent = new Student { Name = "John Doe" };
            await studentRepository.AddAsync(newStudent);

            // Read
            var john = await studentRepository.GetByIdAsync(newStudent.StudentId);
            Console.WriteLine($"Student ID: {john.StudentId}, Name: {john.Name}");

            // Update
            john.Name = "Jane Smith";
            studentRepository.Update(john);

            // Delete
            studentRepository.Delete(john);

            await context.SaveChangesAsync();
        }
    }
}

```


## Exercicio

- 01 Aplicar EF no projeto

 ## Próximos

- [próximo](aula13.md)