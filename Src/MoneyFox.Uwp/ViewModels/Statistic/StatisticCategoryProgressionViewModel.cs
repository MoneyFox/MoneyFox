﻿using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using Microcharts;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Categories;
using MoneyFox.Uwp.ViewModels.Statistics;
using MoneyFox.Uwp.Views.Payments;
using SkiaSharp;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels.Statistic
{
    /// <summary>
    ///     Representation of the cash flow view.
    /// </summary>
    public class StatisticCategoryProgressionViewModel : StatisticViewModel
    {
        private BarChart chart = new BarChart();
        private bool hasNoData = true;
        private CategoryViewModel selectedCategory;
        private readonly IMapper mapper;

        public StatisticCategoryProgressionViewModel(
            IMediator mediator,
            IMapper mapper) : base(mediator)
        {
            this.mapper = mapper;

            StartDate = DateTime.Now.AddYears(-1);
        }

        public CategoryViewModel SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if(selectedCategory == value)
                {
                    return;
                }

                selectedCategory = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Chart to render.
        /// </summary>
        public BarChart Chart
        {
            get => chart;
            set
            {
                if(chart == value)
                {
                    return;
                }

                chart = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Chart to render.
        /// </summary>
        public bool HasNoData
        {
            get => hasNoData;
            set
            {
                if(hasNoData == value)
                {
                    return;
                }

                hasNoData = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand LoadDataCommand => new RelayCommand(async () => await LoadAsync());

        public RelayCommand GoToSelectCategoryDialogCommand => new RelayCommand(
            async ()
                => await new SelectCategoryDialog {RequestedTheme = ThemeSelectorService.Theme}.ShowAsync());

        public RelayCommand ResetCategoryCommand => new RelayCommand(() => SelectedCategory = null);

        protected override async Task LoadAsync()
        {
            if(SelectedCategory == null)
            {
                HasNoData = true;
                return;
            }

            IImmutableList<StatisticEntry> statisticItems =
                await Mediator.Send(new GetCategoryProgressionQuery(SelectedCategory?.Id ?? 0, StartDate, EndDate));

            HasNoData = !statisticItems.Any();

            Chart = new BarChart
            {
                Entries = statisticItems.Select(
                                            x => new ChartEntry((float)x.Value)
                                            {
                                                Label = x.Label,
                                                ValueLabel = x.ValueLabel,
                                                Color = SKColor.Parse(x.Color),
                                                ValueLabelColor = SKColor.Parse(x.Color)
                                            })
                                        .ToList(),
                BackgroundColor = new SKColor(
                    ChartOptions.BackgroundColor.R,
                    ChartOptions.BackgroundColor.G,
                    ChartOptions.BackgroundColor.B,
                    ChartOptions.BackgroundColor.A),
                Margin = ChartOptions.Margin,
                LabelTextSize = ChartOptions.LabelTextSize,
                Typeface = SKTypeface.FromFamilyName(ChartOptions.TypeFace)
            };
        }
    }
}