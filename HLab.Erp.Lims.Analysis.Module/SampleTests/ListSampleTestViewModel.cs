﻿using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using HLab.DependencyInjection.Annotations;
using HLab.Erp.Core.ViewModels;
using HLab.Erp.Core.ViewModels.EntityLists;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Icons;

namespace HLab.Erp.Lims.Analysis.Module.SampleTests
{
    public class ListSampleTestViewModel : EntityListViewModel<ListSampleTestViewModel, SampleTest>, IMvvmContextProvider
    {
        [Import]
        private readonly IIconService _icons;

        private async Task<object> GetIcon(int state,double size)
        {
            switch(state)
            {
                case 1:
                    return await _icons.GetIcon("icons/Results/Gauge", size).ConfigureAwait(false);
                case 2:
                    return await _icons.GetIcon("icons/Results/GaugeKo",size).ConfigureAwait(false);
                case 3:
                    return await _icons.GetIcon("icons/Results/GaugeOk",size).ConfigureAwait(false);
                default:
                    return await _icons.GetIcon("icons/Results/Gauge",size).ConfigureAwait(false);
            }
        }
        private async Task<object> GetCheckIcon(int state,double size)
        {
            switch (state)
            {
                case 1:
                    return await _icons.GetIcon("icons/Results/Running",size).ConfigureAwait(false);
                case 2:
                    return await _icons.GetIcon("icons/Results/CheckFailed",size).ConfigureAwait(false);
                case 3:
                    return await _icons.GetIcon("icons/Results/CheckPassed",size).ConfigureAwait(false);
                default:
                    return await _icons.GetIcon("icons/Results/Running",size).ConfigureAwait(false);
            }
        }

        public ListSampleTestViewModel(int sampleId)
        {
            List.AddFilter(()=>e => e.SampleId == sampleId);
            // List.AddOnCreate(h => h.Entity. = "<Nouveau Critère>").Update();
            Columns
                .Column("",async s => await _icons.GetIcon(s.TestClass.IconPath, 25.0), s => s.TestClass.Order)
                .Column("^Test", s => new StackPanel{
                    VerticalAlignment = VerticalAlignment.Top,
                    Children =
                    {
                        new TextBlock{Text=s.TestName,FontWeight = FontWeights.Bold},
                        new TextBlock{Text = s.Description, FontStyle = FontStyles.Italic}
                    }})
                .Column("^Specifications", s => s.Specification)
                .Column("^Result", s => s.Result)
            //.Column("Conformity", s => s.TestStateId);
                .Column("^State", async s => await GetIcon(s.TestStateId??0,25), s => s.TestStateId)
                .Column("^Validation", async s => await GetCheckIcon(s.Validation??0,25), s => s.Validation)
                .Hidden("IsValid", s => s.Validation!=2)
                .Hidden("Group", s => s.TestClassId);

            List.Update();
        }

        public string Title => "Samples";
        public void ConfigureMvvmContext(IMvvmContext ctx)
        {
        }
    }

}