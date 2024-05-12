using DB_Chatbot.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Data;

namespace Server.Controllers;

[ApiController]
[Route("api/v1/person")]
[Produces("application/json")]
public class PersonController : ControllerBase
{
    private readonly ServerContext _context;

    public PersonController(ServerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get a Person by the given Id
    /// </summary>
    /// <param name="Id">The Person's Id</param>
    /// <returns>Person</returns>
    /// <response code="200">The Person with the given Id</response>
    /// <response code="404">There was no Person with the given Id</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [HttpGet("{Id}")]
    public async Task<ActionResult> ReadPerson(string id)
    {
        var person = await _context.People.FindAsync(id);
        if (person == null)
        {
            return NotFound("Person does not exist");
        }

        return Ok(person);
    }

    /// <summary>
    /// Gets People within the given filters
    /// </summary>
    /// <remarks>
    /// Does not return Not Found, but an Array of size 0 instead
    /// </remarks>
    /// <param name="name">String within the people's name</param>
    /// <param name="minAge">People's minimum age</param>/// 
    /// <param name="maxAge">People's maximum age</param>
    /// <param name="sort">Sort's by the given property's name, Ascending or Descending</param>
    /// <param name="offset">Offsets the result by a given amount</param>
    /// <param name="limit">Limits the result by a given amount</param>
    /// <returns>Person[]</returns>
    /// <response code="200">Returns an array of Person</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Person[]>))]
    [HttpGet]
    public IActionResult ReadPeople(string? name, float? minAge, float? maxAge,
                                    string? sort, int? offset, int? limit)
    {

        var peopleQuery = _context.People.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            peopleQuery = peopleQuery.Where(x => x.FullName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        if (minAge.HasValue)
        {
            peopleQuery = peopleQuery.Where(x => x.age >= minAge);
        }
        if (maxAge.HasValue)
        {
            peopleQuery = peopleQuery.Where(x => x.age <= maxAge);
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            sort = sort.ToLower();
            if (sort.Contains("name"))
            {
                peopleQuery = peopleQuery.OrderBy(s => s.FullName).ThenBy(s => s.age).ThenBy(s => s.Id);
            }
            else if (sort.Contains("age"))
            {
                peopleQuery = peopleQuery.OrderBy(s => s.age).ThenBy(s => s.FullName).ThenBy(s => s.Id);
            }
        }

        if (!string.IsNullOrWhiteSpace(sort) && sort.Contains("desc", StringComparison.OrdinalIgnoreCase))
        {
            peopleQuery.Reverse();
        }

        var resultsArray = peopleQuery
            .Skip(offset ?? 0)
            .Take(limit ?? 10)
            .ToArray();

        return Ok(resultsArray);
    }

    /// <summary>
    /// Create a Person
    /// </summary>
    /// <returns>Person</returns>
    /// <response code="200">The created Person</response>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Person))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpPost]
    public async Task<IActionResult> CreatePerson([FromBody] Person newPerson)
    {

        newPerson.Id = Guid.NewGuid().ToString();

        _context.People.Add(newPerson);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(CreatePerson), newPerson);
    }

    /// <summary>
    /// Update the Person with the given Id
    /// </summary>
    /// <returns>Person</returns>
    /// <response code="200">The updated Person</response>
    /// <response code="400">There was no Person with the given Id</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpPut]
    public async Task<IActionResult> UpdatePerson([FromBody] Person uPerson)
    {

        var existingPerson = _context.People.Find(uPerson.Id);
        if (existingPerson == null)
        {
            return BadRequest("Person does not exist");
        }

        existingPerson.FullName = uPerson.FullName;
        existingPerson.age = uPerson.age;

        _context.People.Update(uPerson);
        await _context.SaveChangesAsync();

        return Ok(uPerson);
    }

    /// <summary>
    /// Deletes the Person with the given Id
    /// </summary>
    /// <param name="Id">The Id of Person to be deleted</param>
    /// <returns>NoContentResult</returns>
    /// <response code="204">No Content</response>
    /// <response code="400">There was no Person with the given Id</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeletePerson(string id)
    {
        var person = _context.People.Find(id);
        if (person == null)
        {
            return NotFound("Person does not exist");
        }

        _context.People.Remove(person);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
