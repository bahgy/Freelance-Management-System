using Freelance_Bot.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Freelance_Bot.Domain.Entity
{
    public class Project : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        public Guid? ClientId { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Budget { get; set; }
        public string Currency { get; set; } = "USD";

        public DateTime? StartDate { get; set; }
        public DateTime? Deadline { get; set; }
        public int ProgressPct { get; set; } = 0;

        // Navigation
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [ForeignKey(nameof(ClientId))]
        public Client? Client { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = [];
        public ICollection<Report> Reports { get; set; } = [];
        public ICollection<Insight> Insights { get; set; } = [];
    }
}
