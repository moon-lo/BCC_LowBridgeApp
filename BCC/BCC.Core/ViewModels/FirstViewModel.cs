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
        public ICommand ButtonCommand { get; private set; }
        public ICommand SelectSearchCommand { get; private set; }
        public FirstViewModel()
        {
            SelectSearchCommand = new MvxCommand<Result>(search =>
            {
                SearchResult = search.SearchResult;
            });
        }

        private string searchResult;
        public string SearchResult
        {
            get { return searchResult; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref searchResult, value);
                    AddUnit(new Result(SearchResult));
                    RaisePropertyChanged(() => SearchData);
                }
            }
        }

        public void AddUnit(Result search)
        {
            if (search.SearchResult != null)
            {
                if (search.SearchResult.Trim() != string.Empty)
                    SearchData.Add(search);
                else
                    //Note this code just removes spaces from the EditText if that is all was in them
                    SearchResult = search.SearchResult;
            }
        }

        private ObservableCollection<Result> searchData = new ObservableCollection<Result>();
        public ObservableCollection<Result> SearchData
        {
            get { return searchData; }
            set
            {
                SetProperty(ref searchData, value);
            }
        }
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
}
