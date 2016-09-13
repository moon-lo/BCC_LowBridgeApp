using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BCC.Core.ViewModels
{
    /// <summary>
    /// Author: N9452982, Michael Devenish
    /// </summary>
    public class FirstViewModel
        : MvxViewModel
    {

        private ObservableCollection<Unit> unitCodes;
        public ObservableCollection<Unit> UnitCodes
        {
            get { return unitCodes; }
            set { SetProperty(ref unitCodes, value); }
        }

        private string unitCode;
        public string UnitCode
        {
            get { return unitCode; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref unitCode, value);
                    //UnitCodes.Clear();
                    AddUnit(new Unit(UnitCode));
                    RaisePropertyChanged(() => UnitCodes);
                }
            }
        }

        public ICommand ButtonCommand { get; private set; }
        public ICommand SelectUnitCommand { get; private set; }
        public FirstViewModel()
        {
            UnitCodes = new ObservableCollection<Unit>(){
            new Unit("IAB330") ,
            new Unit() { UnitCode="IAB230"}
        };

            ButtonCommand = new MvxCommand(() =>
            {
                AddUnit(new Unit(UnitCode));
                RaisePropertyChanged(() => UnitCodes);
            });
            SelectUnitCommand = new MvxCommand<Unit>(unit =>
            {
                UnitCode = unit.UnitCode;
            });
        }


        public void AddUnit(Unit unit)
        {
            if (unit.UnitCode != null)
            {
                if (unit.UnitCode.Trim() != string.Empty)
                    UnitCodes.Add(unit);
                else
                    //Note this code just removes spaces from the EditText if that is all was in them
                    UnitCode = UnitCode.Trim();
            }
        }

    }

    public class Unit
    {
        public string UnitCode { get; set; }
        public Unit() { }
        public Unit(string unitCode)
        {
            UnitCode = unitCode;
        }
    }
}
