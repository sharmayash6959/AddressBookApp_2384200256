using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class AddressBookRL : IAddressBookRL
    {
        private readonly AddressBookContext _context;

        public AddressBookRL(AddressBookContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResponseModel>> GetAllContacts()
        {
            return await _context.AddressBooks
                .Select(c => new ResponseModel { Id = c.Id, Name = c.Name, Email = c.Email, Phone = c.Phone, Address = c.Address })
                .ToListAsync();
        }

        public async Task<ResponseModel> GetContactById(int id)
        {
            var contact = await _context.AddressBooks.FindAsync(id);
            if (contact == null) return null;

            return new ResponseModel { Id = contact.Id, Name = contact.Name, Email = contact.Email, Phone = contact.Phone, Address = contact.Address };
        }

        public async Task<ResponseModel> AddContact(RequestModel request)
        {
            var contact = new Entity.AddressBook { Name = request.Name, Email = request.Email, Phone = request.Phone, Address = request.Address };
            _context.AddressBooks.Add(contact);
            await _context.SaveChangesAsync();

            return new ResponseModel { Id = contact.Id, Name = contact.Name, Email = contact.Email, Phone = contact.Phone, Address = contact.Address };
        }

        public async Task<bool> UpdateContact(int id, RequestModel request)
        {
            var contact = await _context.AddressBooks.FindAsync(id);
            if (contact == null) return false;

            contact.Name = request.Name;
            contact.Email = request.Email;
            contact.Phone = request.Phone;
            contact.Address = request.Address;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteContact(int id)
        {
            var contact = await _context.AddressBooks.FindAsync(id);
            if (contact == null) return false;

            _context.AddressBooks.Remove(contact);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddContactAsync(AddressBookEntry entry)
        {
            try
            {
                var contact = new Entity.AddressBook
                {
                    Name = entry.Name,
                    Email = entry.Email,
                    Phone = entry.Phone,
                    Address = entry.Address
                };
                _context.AddressBooks.Add(contact);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<AddressBookEntry> AddContactAsync(AddressBookEntry entry)
        {
            try
            {
                _context.AddressBooks.Add(entry);
                await _context.SaveChangesAsync();
                return entry;  // Return the newly added contact
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null; // Return null in case of an error
            }
        }

        public async Task<AddressBookEntry?> GetContactByIdAsync(int id)
        {
            return await _context.AddressBooks.FindAsync(id) ?? null;
        }

    }
}
