using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AddressBookBL : IAddressBookBL
    {
        private readonly IAddressBookRL _addressBookRL;

        public AddressBookBL(IAddressBookRL addressBookRL)
        {
            _addressBookRL = addressBookRL;
        }

        public async Task<IEnumerable<ResponseModel>> GetAllContacts()
        {
            return await _addressBookRL.GetAllContacts();
        }

        public async Task<ResponseModel> GetContactById(int id)
        {
            return await _addressBookRL.GetContactById(id);
        }

        public async Task<ResponseModel> AddContact(RequestModel request)
        {
            return await _addressBookRL.AddContact(request);
        }

        public async Task<bool> UpdateContact(int id, RequestModel request)
        {
            return await _addressBookRL.UpdateContact(id, request);
        }

        public async Task<bool> DeleteContact(int id)
        {
            return await _addressBookRL.DeleteContact(id);
        }
    }
}
