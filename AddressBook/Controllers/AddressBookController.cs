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
    private readonly IAddressBookRL _addressBookService;
    private readonly IAddressBookBL _addressBookBL;
    private readonly IMapper _mapper;
    private readonly IValidator<AddressBookDTO> _validator;

    public AddressBookController(
        IAddressBookRL addressBookService,
        IAddressBookBL addressBookBL,
        IMapper mapper,
        IValidator<AddressBookDTO> validator)
    {
        _addressBookService = addressBookService ?? throw new ArgumentNullException(nameof(addressBookService));
        _addressBookBL = addressBookBL ?? throw new ArgumentNullException(nameof(addressBookBL));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    [HttpPost]
    public async Task<IActionResult> AddContact([FromBody] AddressBookDTO addressBookDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(addressBookDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var addressBookEntry = _mapper.Map<AddressBookEntry>(addressBookDto);
            var result = await _addressBookService.AddContactAsync(addressBookEntry);
            return CreatedAtAction(nameof(GetContactByIdFromRepository), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContactByIdFromRepository(int id)
    {
        try
        {
            var contact = await _addressBookService.GetContactByIdAsync(id);
            if (contact == null)
                return NotFound();

            var contactDto = _mapper.Map<AddressBookDTO>(contact);
            return Ok(contactDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
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
    [HttpGet("business/{id}")]
    public async Task<ActionResult<ResponseModel>> GetContactByIdFromBusinessLayer(int id)
    {
        try
        {
            var contact = await _addressBookBL.GetContactById(id);
            if (contact == null) return NotFound();

            return new ResponseModel
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                Phone = contact.Phone,
                Address = contact.Address
            };
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
    public async Task<ActionResult<ResponseModel>> AddContactBusinessLayer(RequestModel request)
    {
        try
        {
            var createdContact = await _addressBookBL.AddContact(request);
            return CreatedAtAction(nameof(GetContactByIdFromBusinessLayer), new { id = createdContact.Id }, createdContact);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    /// <summary>
    /// Update contact
    /// </summary>
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateContact(int id, RequestModel request)
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
    /// Delete contact
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
