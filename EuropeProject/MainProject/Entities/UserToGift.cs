using ORM;

namespace MainProject.Entities
{
    public class UserToGift : Entity
    {
        public string UserName { get; set; }
        public string GiftName { get; set; }
    }
}
