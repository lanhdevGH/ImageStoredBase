namespace ImageStoreBase.Api.DTOs.PermissionDTO
{
    public class PermissionScreenResponseDTO
    {
        public string Id { get; set; } // ID của Function
        public string Name { get; set; } // Tên Function
        public string ParentId { get; set; } // ID cha (nếu có)
        public Dictionary<string, int> Commands { get; set; } // Danh sách Command và số lần xuất hiện
    }
}
