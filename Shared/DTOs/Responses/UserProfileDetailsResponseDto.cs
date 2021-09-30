using System;
using System.Collections.Generic;

namespace Quanda.Shared.DTOs.Responses
{
    public class UserProfileDetailsResponseDto
    {
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public string Bio { get; set; }

        public bool IsPremium { get; set; }

        public int Points { get; set; }
        public int Questions { get; set; }
        public int Answers { get; set; }
        public int ProfileViews { get; set; }
        public int? TopUsersPlacement { get; set; }

        public DateTime RegisteredAt { get; set; }

        public IEnumerable<UserInTagResponseDto> TopTags { get; set; }
    }
}
