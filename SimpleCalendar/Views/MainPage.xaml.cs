using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using SimpleCalendar.Models;
using SimpleCalendar.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCalendar.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalStorageService _storageService = new LocalStorageService();
        private List<CalendarItem> _allItems = new List<CalendarItem>();
        public DateTime SelectedDate { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            SelectedDate = DateTime.Today;
            datePicker.Date = SelectedDate;
        }

        // Загрузка сохраненных заметок при каждом открытии экрана
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _allItems = await _storageService.LoadItemsAsync();
            RefreshData();
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
           
            SelectedDate = e.NewDate ?? DateTime.Today;
            RefreshData();
        }

        private void RefreshData()
        {
            selectedDateLabel.Text = $"Записи на {SelectedDate:dd.MM.yyyy}:";

            // Фильтруем записи по выбранному дню
            var filteredItems = _allItems
                .Where(x => x.Date.Date == SelectedDate.Date)
                .ToList();

            itemsCollectionView.ItemsSource = filteredItems;
        }

        //Добавление элемента (стикера) 
        private string _selectedSticker = string.Empty;

        private void StickerSelected(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                _selectedSticker = button.CommandParameter?.ToString() ?? string.Empty;

            }
        }

        // Добавление быстрой заметки
        private async void AddNote_Clicked(object sender, EventArgs e)
        {
            string text = await DisplayPromptAsync("Новая заметка", "Введите текст заметки:");
            if (string.IsNullOrWhiteSpace(text)) return;

            var item = new CalendarItem
            {
                Title = text,
                Date = SelectedDate,
                TimeText = "Заметка (Весь день)",
                Category = "Заметка",
                Sticker = _selectedSticker // Добавляем стикер
            };

            _allItems.Add(item);
            await _storageService.SaveItemsAsync(_allItems);
            RefreshData();
        }

        // Добавление события со временем
        private async void AddEvent_Clicked(object sender, EventArgs e)
        {
            string title = await DisplayPromptAsync("Новое событие", "Название события (например: Встреча):");
            if (string.IsNullOrWhiteSpace(title)) return;

            string time = await DisplayPromptAsync("Время", "Укажите время (например: 15:00):", initialValue: "12:00");
            if (string.IsNullOrWhiteSpace(time)) time = "Весь день";

            var item = new CalendarItem
            {
                Title = title,
                Date = SelectedDate,
                TimeText = $"Время: {time}",
                Category = "Событие"
            };

            _allItems.Add(item);
            await _storageService.SaveItemsAsync(_allItems);
            RefreshData();
        }

        // Удаление элемента
        private async void DeleteKey_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is CalendarItem itemToDelete)
            {
                bool confirm = await DisplayAlert("Удаление", $"Удалить \"{itemToDelete.Title}\"?", "Да", "Нет");
                if (confirm)
                {
                    _allItems.Remove(itemToDelete);
                    await _storageService.SaveItemsAsync(_allItems);
                    RefreshData();
                }
            }
        }
    }
}