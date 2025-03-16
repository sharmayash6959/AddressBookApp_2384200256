using System;
using AutoMapper;
using BusinessLayer.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AddressBookController : ControllerBase
{
    private readonly IAddressBookBL _addressBookBL;
    private readonly IMapper _mapper;
    private readonly IValidator<AddressBookDTO> _validator;

    public AddressBookController(
        IAddressBookBL addressBookBL,
        IMapper mapper,
        IValidator<AddressBookDTO> validator)
    {
        _addressBookBL = addressBookBL ?? throw new ArgumentNullException(nameof(addressBookBL));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    /// <summary>
    /// Retrieves all contacts
    /// </summary>
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ResponseModel>>> GetContacts()
    {
        try
        {
            var contacts = await _addressBookBL.GetAllContacts();
            return Ok(contacts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Get contact by id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetContactById(int id)
    {
        try
        {
            var contact = await _addressBookBL.GetContactById(id);
            if (contact == null) return NotFound();

            var contactDto = _mapper.Map<AddressBookDTO>(contact);
            return Ok(contactDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Adds a new contact
    /// </summary>
    [HttpPost("add")]
    public async Task<ActionResult<ResponseModel>> AddContact([FromBody] AddressBookDTO addressBookDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(addressBookDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var addressBookEntry = _mapper.Map<AddressBookEntry>(addressBookDto);
            var createdContact = await _addressBookBL.AddContact(addressBookEntry);
            return CreatedAtAction(nameof(GetContactById), new { id = createdContact.Id }, createdContact);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing contact
    /// </summary>
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateContact(int id, [FromBody] RequestModel request)
    {
        try
        {
            var updated = await _addressBookBL.UpdateContact(id, request);
            if (!updated)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a contact
    /// </summary>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        try
        {
            var deleted = await _addressBookBL.DeleteContact(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
