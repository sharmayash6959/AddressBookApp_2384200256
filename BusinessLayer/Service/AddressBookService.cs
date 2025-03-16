using BusinessLayer.Interface;
using ModelLayer.DTOs;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AddressBookService : IAddressBookService
    {
        private readonly IAddressBookRL _addressBookRepository;

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
