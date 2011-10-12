using System;

namespace CS_AspNetMvc3_WithoutJsSdk.Models
{
    public class FacebookUser
    {
        public string FacebookId { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expires { get; set; }

        public string Name { get; set; }
    }
}