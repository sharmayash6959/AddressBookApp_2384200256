using ModelLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IAddressBookRL
    {

        Task<AddressBookEntry> AddContactAsync(AddressBookEntry entry);
        Task<AddressBookEntry?> GetContactByIdAsync(int id);


        Task<IEnumerable<ResponseModel>> GetAllContacts();
        Task<ResponseModel> GetContactById(int id);
        Task<ResponseModel> AddContact(RequestModel request);
        Task<bool> UpdateContact(int id, RequestModel request);
        Task<bool> DeleteContact(int id);
    }
}
