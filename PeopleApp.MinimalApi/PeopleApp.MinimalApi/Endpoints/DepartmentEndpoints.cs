using PeopleApp.MinimalApi.Models;
using static PeopleApp.MinimalApi.Program;

public static class DepartmentEndpoints
{
    public static void MapDepartmentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/departments").WithTags("Department");
        group.MapGet("/", GetDepartments);
        group.MapGet("/{id}", GetDepartment);
        group.MapPost("/", CreateDepartment);
    }

    private static IEnumerable<Department> GetDepartments()
    {
        return new List<Department>
        {
            new Department { Id = 1, Name = "HR" },
            new Department { Id = 2, Name = "IT" }
        };
    }

    private static IResult GetDepartment(int id)
    {
        var departments = GetDepartments();
        var department = departments.FirstOrDefault(d => d.Id == id);

        if (department == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(department);
    }

    private static IResult CreateDepartment(Department department)
    {
        return Results.Created($"/departments/{department.Id}", department);
    }
}