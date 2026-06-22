using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance_Bot.Domain.Entity
{
    public class User: BaseEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }// To use it Later for authentication 
        public long? TelegramChatId { get; set; }

        // *******************to use it later for scheduling and notifications and Plan *******************
        //public string Timezone { get; set; } = "UTC";
        //public string Plan { get; set; } = "free"; // free, pro, team
        public ICollection<Client> Clients { get; set; } = [];
        public ICollection<Project> Projects { get; set; } = [];
        public ICollection<Insight> Insights { get; set; } = [];
        public ICollection<AppEvent> Events { get; set; } = [];
        public ICollection<Notification> Notifications { get; set; } = [];
    }
}
