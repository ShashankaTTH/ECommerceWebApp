using System.Collections.Generic;
using System.Threading.Tasks;
using EComerceWebApp.Model;
using static EComerceWebApp.Components.Pages.GetItems;
//using static EComerceWebApp.Components.Pages.GetItems;

namespace EComerceWebApp.Interface
{
    public interface IItemRepository
    {
        List<Model.Item> GetItems();

        string getItemImageById(int Id);
        public bool AddToCart(CartItem cartItem);
    }

}
