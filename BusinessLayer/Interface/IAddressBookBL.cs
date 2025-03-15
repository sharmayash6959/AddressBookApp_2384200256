using ModelLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAddressBookBL
    {
        Task<IEnumerable<ResponseModel>> GetAllContacts();
        Task<ResponseModel> GetContactById(int id);
        Task<ResponseModel> AddContact(RequestModel request);
        Task<bool> UpdateContact(int id, RequestModel request);
        Task<bool> DeleteContact(int id);
    }
}
