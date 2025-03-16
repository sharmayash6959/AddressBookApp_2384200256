using ModelLayer.DTOs;
using ModelLayer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAddressBookService
    {
        Task<IEnumerable<ResponseModel>> GetAllContactsAsync();
        Task<ResponseModel?> GetContactByIdAsync(int id);
        Task<ResponseModel> AddContactAsync(RequestModel request);
        Task<bool> UpdateContactAsync(int id, RequestModel request);
        Task<bool> DeleteContactAsync(int id);
    }
}
