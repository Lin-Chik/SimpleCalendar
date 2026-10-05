using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using SimpleCalendar.Models;

namespace SimpleCalendar.Services
{
    public class LocalStorageService
    {
        // Путь к файлу в изолированном хранилище приложения на телефоне
        private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "calendar_data.json");

        public async Task SaveItemsAsync(List<CalendarItem> items)
        {
            try
            {
                string json = JsonSerializer.Serialize(items);
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения: {ex.Message}");
            }
        }

        public async Task<List<CalendarItem>> LoadItemsAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<CalendarItem>();

                string json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<CalendarItem>>(json) ?? new List<CalendarItem>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки: {ex.Message}");
                return new List<CalendarItem>();
            }
        }
    }
}