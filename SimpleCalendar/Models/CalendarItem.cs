using System;

namespace SimpleCalendar.Models
{
    public class CalendarItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string TimeText { get; set; } // Например: "14:30" или "Весь день"
        public string Category { get; set; } // "Заметка" или "Событие"
        public string Sticker { get; set; } = string.Empty; // Добавление стикеров, например: "🎉", "🔥", "📌" 

        public override string ToString()
        {
            return $"[{Category}] {TimeText} — {Title}";
        }
    }
}