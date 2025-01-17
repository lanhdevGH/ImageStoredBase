namespace ImageStoreBase.Api.DTOs.UserDTOs
{
    public class ChangePasswordRequestDTO
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
