using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicEquipmentSystem.ViewModels
{
    public class ListOfFriendsOwnersViewModel
    {
        public List<FriendOwnerModelView> List { get; set; }

        public ListOfFriendsOwnersViewModel()
        {
            List = new List<FriendOwnerModelView>();
        }
    }
}
