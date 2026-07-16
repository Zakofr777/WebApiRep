using FluentValidation;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly FileService _fileService;
    private readonly IValidator<Person> _validator;

    public PersonController(FileService fileService, IValidator<Person> validator)
    {
        _fileService = fileService;
        _validator = validator;
    }

    [HttpPost]
    public IActionResult Create([FromBody] Person person)
    {
        var validationResult = _validator.Validate(person);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
        }

        var updatedList = _fileService.Add(person);
        return Ok(updatedList);
    }

    [HttpGet]
    public IActionResult getAll()
    {
        var people = _fileService.GetAll();
        return Ok(people);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetByIndex(int id)
    {
        var person = _fileService.GetByIndex(id);
        if (person == null)
        {
            return NotFound(new { Message = $"ჩანაწერი ინდექსით {id} ვერ მოიძებნა." });
        }
        return Ok(person); 
    }
    
    [HttpGet("filter")]
    public IActionResult GetFiltered([FromQuery] double? minSalary, [FromQuery] string? city)
    {
        var people = _fileService.GetAll();
        
        if (minSalary.HasValue)
        {
            people = people.Where(p => p.Salary >= minSalary.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            people = people.Where(p => p.PersonAddress != null && 
                                       p.PersonAddress.City.Contains(city, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return Ok(people);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var updatedList = _fileService.DeleteByIndex(id);
        if (updatedList == null)
        {
            return NotFound(new { Message = $"ვერ მოხდა წაშლა. ჩანაწერი ინდექსით {id} ვერ მოიძებნა." });
        }
        return Ok(updatedList); 
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Person updatedPerson)
    {
        var validationResult = _validator.Validate(updatedPerson);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
        }

        var updatedList = _fileService.UpdateByIndex(id, updatedPerson);
        if (updatedList == null)
        {
            return NotFound(new { Message = $"ვერ მოხდა ჩანაცვლება. ჩანაწერი ინდექსით {id} ვერ მოიძებნა." });
        }
        return Ok(updatedList); 
    }
}