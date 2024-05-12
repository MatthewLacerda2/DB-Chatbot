using DB_Chatbot.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Server.Data;

namespace Server.Controllers;

[ApiController]
[Route("api/v1/item")]
[Produces("application/json")]
public class ItemController : ControllerBase
{
    private readonly ServerContext _context;

    public ItemController(ServerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get an Item by the given Id
    /// </summary>
    /// <param name="Id">The Item's Id</param>
    /// <returns>AptLog</returns>
    /// <response code="200">The Item with the given Id</response>
    /// <response code="404">There was no Item with the given Id</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Item))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [HttpGet("{Id}")]
    public async Task<ActionResult> GetItem(string id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item == null)
        {
            return NotFound("Item does not exist");
        }

        return Ok(item);
    }

    /// <summary>
    /// Gets Item within the given filters
    /// </summary>
    /// <remarks>
    /// Does not return Not Found, but an Array of size 0 instead
    /// </remarks>
    /// <param name="name">String within the Item's name</param>
    /// <param name="description">String within the Item's description</param>
    /// <param name="personId">Item's owner. Use empty string if you want Items without an owner</param>/// 
    /// <param name="minPreco">Item's minimum price</param>
    /// <param name="maxPreco">Item's maximum price</param>
    /// <param name="sort">Sort the items by the given property's name, Ascending or Descending</param>
    /// <param name="offset">Offsets the result by a given amount</param>
    /// <param name="limit">Limits the result by a given amount</param>
    /// <returns>Item[]</returns>
    /// <response code="200">Returns an array of Item</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Item[]>))]
    [HttpGet]
    public IActionResult ReadItems(string? name, string? description, string? personId,
                                    float? minPreco, float? maxPreco,
                                    string? sort, int? offset, int? limit)
    {

        var itemQuery = _context.Items.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            itemQuery = itemQuery.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            itemQuery = itemQuery.Where(x => x.Descricao.Contains(description, StringComparison.OrdinalIgnoreCase));
        }

        if (personId != null)
        {
            itemQuery = itemQuery.Where(x => x.PersonId == personId);
        }

        if (minPreco.HasValue)
        {
            itemQuery = itemQuery.Where(x => x.preco >= minPreco);
        }
        if (maxPreco.HasValue)
        {
            itemQuery = itemQuery.Where(x => x.preco <= maxPreco);
        }

        if (!string.IsNullOrWhiteSpace(sort))
        {
            sort = sort.ToLower();
            if (sort.Contains("name"))
            {
                itemQuery = itemQuery.OrderBy(s => s.Name).ThenBy(s => s.preco).ThenBy(s => s.Id);
            }
            else if (sort.Contains("person"))
            {
                itemQuery = itemQuery.OrderBy(s => s.preco).ThenBy(s => s.Name).ThenBy(s => s.Id);
            }
        }

        if (!string.IsNullOrWhiteSpace(sort) && sort.Contains("desc", StringComparison.OrdinalIgnoreCase))
        {
            itemQuery.Reverse();
        }

        var resultsArray = itemQuery
            .Skip(offset ?? 0)
            .Take(limit ?? 10)
            .ToArray();

        return Ok(resultsArray);
    }

    /// <summary>
    /// Create an Item
    /// </summary>
    /// <returns>Item</returns>
    /// <response code="200">The created Item</response>
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Item))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpPost]
    public async Task<IActionResult> CreateLog([FromBody] Item newItem)
    {

        newItem.Id = Guid.NewGuid().ToString();

        _context.Items.Add(newItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(CreateLog), newItem);
    }

    /// <summary>
    /// Update the Item with the given Id
    /// </summary>
    /// <returns>Person</returns>
    /// <response code="200">The updated Item</response>
    /// <response code="400">There was no Item with the given Id</response>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Item))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpPut]
    public async Task<IActionResult> UpdateItem([FromBody] Item upItem)
    {

        var existingItem = _context.Items.Find(upItem.Id);
        if (existingItem == null)
        {
            return BadRequest("Item does not exist");
        }

        if (upItem.PersonId != string.Empty)
        {

            var existingPerson = _context.People.Find(upItem.PersonId);
            if (existingPerson == null)
            {
                return BadRequest("Person does not exist");
            }
        }

        existingItem.PersonId = upItem.PersonId;
        existingItem.Name = upItem.Name;
        existingItem.Descricao = upItem.Descricao;
        existingItem.preco = upItem.preco;

        _context.Items.Update(upItem);
        await _context.SaveChangesAsync();

        return Ok(upItem);
    }

    /// <summary>
    /// Deletes the Item with the given Id
    /// </summary>
    /// <param name="Id">The Id of Item to be deleted</param>
    /// <returns>NoContentResult</returns>
    /// <response code="204">No Content</response>
    /// <response code="400">There was no Item with the given Id</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeletePerson(string id)
    {
        var item = _context.Items.Find(id);
        if (item == null)
        {
            return NotFound("Item does not exist");
        }

        _context.Items.Remove(item);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
