using Microsoft.AspNetCore.Mvc;
using SWARM.EF.Models;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers
{
    public interface IBaseController<T>
    {
        Task<IActionResult> Delete(int itemID);
        Task<IActionResult> Get();
        Task<IActionResult> Get(int itemID);
        Task<IActionResult> Post([FromBody] T item);
        Task<IActionResult> Put([FromBody] T item);
    }
}