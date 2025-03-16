using BusinessLayer.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using ModelLayer.DTOs;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AddressBookService : IAddressBookService
    {
        private readonly IAddressBookRL _addressBookRepository;

        //
        private readonly IDatabase _cache;

        public AddressBookService(IAddressBookRL addressBookRepository, IConnectionMultiplexer redis)
        {
            _addressBookRepository = addressBookRepository;
            _cache = redis.GetDatabase();
        }

        public async Task<IEnumerable<ResponseModel>> GetAllContactsAsync()
        {
            string cacheKey = "contacts";
            var cachedData = await _cache.StringGetAsync(cacheKey);

            if (!cachedData.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<IEnumerable<ResponseModel>>(cachedData);
            }

            var contacts = await _addressBookRepository.GetAllContacts();
            await _cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(contacts), TimeSpan.FromMinutes(10));

            return contacts;
        }

        public async Task<ResponseModel?> GetContactByIdAsync(int id)
        {
            string cacheKey = $"contact:{id}";
            var cachedData = await _cache.StringGetAsync(cacheKey);

            if (!cachedData.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<ResponseModel>(cachedData);
            }

            var contact = await _addressBookRepository.GetContactByIdAsync(id);
            if (contact != null)
            {
                await _cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(contact), TimeSpan.FromMinutes(10));
            }

            return contact;
        }

        public async Task<ResponseModel> AddContactAsync(RequestModel request)
        {
            var contact = await _addressBookRepository.AddContact(request);
            await _cache.KeyDeleteAsync("contacts"); // Invalidate cache
            return contact;
        }

        public async Task<bool> UpdateContactAsync(int id, RequestModel request)
        {
            var updated = await _addressBookRepository.UpdateContact(id, request);
            if (updated)
            {
                await _cache.KeyDeleteAsync($"contact:{id}");
                await _cache.KeyDeleteAsync("contacts");
            }
            return updated;
        }

        public async Task<bool> DeleteContactAsync(int id)
        {
            var deleted = await _addressBookRepository.DeleteContact(id);
            if (deleted)
            {
                await _cache.KeyDeleteAsync($"contact:{id}");
                await _cache.KeyDeleteAsync("contacts");
            }
            return deleted;
        }
        //

        public AddressBookService(IAddressBookRL addressBookRepository)
        {
            _addressBookRepository = addressBookRepository;
        }

        public async Task<IEnumerable<ResponseModel>> GetAllContactsAsync()
        {
            return await _addressBookRepository.GetAllContacts();
        }

        public async Task<ResponseModel?> GetContactByIdAsync(int id)
        {
            return await _addressBookRepository.GetContactByIdAsync(id);
        }

        public async Task<ResponseModel> AddContactAsync(RequestModel request)
        {
            return await _addressBookRepository.AddContact(request);
        }

        public async Task<bool> UpdateContactAsync(int id, RequestModel request)
        {
            return await _addressBookRepository.UpdateContact(id, request);
        }

        public async Task<bool> DeleteContactAsync(int id)
        {
            return await _addressBookRepository.DeleteContact(id);
        }
    }
}
