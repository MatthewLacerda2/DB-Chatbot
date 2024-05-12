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
    /// <returns>AptLog</returns>
    /// <response code="200">The Person with the given Id</response>
    /// <response code="404">There was no Person with the given Id</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Person))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [HttpGet("{Id}")]
    public async Task<ActionResult> GetPerson(string id)
    {
        var person = await _context.People.FindAsync(id);
        if (person == null)
        {
            return NotFound("Person does not exist");
        }

        return Ok(person);
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
        var person = await _context.People.FindAsync(id);
        if (person == null)
        {
            return NotFound("Person does not exist");
        }

        _context.People.Remove(person);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
