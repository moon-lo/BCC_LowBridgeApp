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

        private string searchResult;
        public string SearchResult
        {
            get { return searchResult; }
            set
            {
                if (value != null)
                {
                    AddUnit(new Result(value));
                    SetProperty(ref searchResult, value);

                }
            }
        }
        public void AddUnit(Result unit)
        {
            if (unit.SearchResult != null)
            {
                if (unit.SearchResult.Trim() != string.Empty)
                {
                    SearchData.Add(unit);
                }
                else
                {
                    //Note this code just removes spaces from the EditText if that is all was in them
                    SearchResult = unit.SearchResult;
                }
            }
        }

        private string searchText = "";
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (value != null && value != searchText)
                {
                    searchText = value;
                    RaisePropertyChanged(() => SearchText);
                    //show results
                }
            }
        }
        private ObservableCollection<Result> searchData;
        public ObservableCollection<Result> SearchData
        {
            get { return searchData; }
            set { SetProperty(ref searchData, value); }
        }
        public class Result
        {
            public string SearchResult { get; set; }
            public Result() { }
            public Result(string result)
            {
                SearchResult = result;
            }
        }
        public ICommand SelectUnitCommand { get; private set; }
        public FirstViewModel()
        {
            SelectUnitCommand = new MvxCommand<Result>(unit =>
            {
                SearchResult = unit.SearchResult;
            });
        }

    }
}
