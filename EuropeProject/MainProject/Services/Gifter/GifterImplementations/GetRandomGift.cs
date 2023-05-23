using MainProject.Core;
using MainProject.Entities;

namespace MainProject.Services.Gifter
{
    public class GetRandomGift : IGifter
    {
        private App _application;
        public GetRandomGift(App app)
        {
            this._application = app;
        }
        public string GenerateGift(User user)
        {
            List<UserToGift> userToGift = _application.DB.GetTable<UserToGift>();
            if (userToGift != null && userToGift.Count == 10 || _application.DB.Exists<UserToGift>(ug => ug.UserName == user.Name)) return null;

            List<Gift> gifts = _application.DB.GetTable<Gift>();
            if (gifts.Count == 0) return null;
            int GeneratedIndex = new Random().Next(0, gifts.Count - 1);
            string GiftName = gifts[GeneratedIndex].GiftName;
            _application.DB.Remove<Gift>(g => g.ID == gifts[GeneratedIndex].ID);
            _application.DB.Add<UserToGift>(new UserToGift() { UserName = user.Name, GiftName = GiftName });
            return GiftName;
        }
    }
}
