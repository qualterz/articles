using Articles.Infrastructure;
using Articles.Model;
using Articles.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Articles.ViewModel
{
    class ExploreTabViewModel : BaseNotifyPropertyChanged
    {
        private readonly User currentUser = LoginViewModel.Instance.User;

        private ContentControl articleView;
        public ContentControl ArticleView
        {
            get => articleView;
            private set
            {
                articleView = value;
                Notify();
            }
        }

        private Article selectedArticle;
        public Article SelectedArticle
        {
            get => selectedArticle;
            set
            {
                selectedArticle = value;
                ArticleChangedCommand.Execute(null);
                Notify();
            }
        }

        private string selectedArticleAuthor;
        public string SelectedArticleAuthor
        {
            get => selectedArticleAuthor;
            set
            {
                selectedArticleAuthor = value;
                Notify();
            }
        }

        private int selectedArticleVotes;
        public int SelectedArticleVotes
        {
            get => selectedArticleVotes;
            set
            {
                selectedArticleVotes = value;
                Notify();
            }
        }

        public bool IsSelectedArticleVoted
        {
            get
            {
                if (SelectedArticle == null)
                    return false;

                using (var db = Database.Get())
                {
                    var votes = db.GetCollection<Vote>();

                    var articleVotes = votes
                        .Query()
                        .Where(
                            v => v.ArticleId.Equals(SelectedArticle.Id
                        )).ToList();

                    var userVotes = articleVotes.FindAll(v => v.UserId == currentUser.Id);

                    if (userVotes.Count == 0)
                        return false;
                }

                return true;
            }
        }

        private List<Article> articles = new List<Article>();
        public ObservableCollection<Article> ViewArticles { get; set; }
            = new ObservableCollection<Article>();

        public ICommand ArticleChangedCommand { get; private set; }
        public ICommand VoteCommand { get; private set; }

        private bool filterOwnFlag = false;
        private bool sortNewestFlag = true;
        private bool sortBestFlag = false;

        public ICommand ToggleFilterOwnCommand { get; private set; }
        public ICommand SwitchSortNewestCommand { get; private set; }
        public ICommand SwitchSortBestCommand { get; private set; }

        public ExploreTabViewModel()
        {
            InitializeCommands();

            ReloadArticles();

            SortNewest();

            ReloadViewArticles();
        }

        private void InitializeCommands()
        {
            ArticleChangedCommand = new RelayCommand(_ =>
            {
                // Update article view
                {
                    var control = new PreviewControl();

                    control.Title = SelectedArticle?.Title;
                    control.Text = SelectedArticle?.Text;

                    ArticleView = control;
                }

                // Count votes
                using (var db = Database.Get())
                {
                    if (SelectedArticle != null)
                    {
                        var votes = db.GetCollection<Vote>();
                        var articleVotes = votes.Query().Where(v =>
                            v.ArticleId == SelectedArticle.Id).Count();

                        SelectedArticleVotes = articleVotes;
                    }
                    else SelectedArticleVotes = 0;
                }

                // Get author name by id 
                using (var db = Database.Get())
                {
                    SelectedArticleAuthor = db.GetCollection<User>()
                        .FindOne(
                            u => u.Id == SelectedArticle.AuthorId
                        ).Name;
                }
            }, _ => { return SelectedArticle != null; });

            VoteCommand = new RelayCommand(_ =>
            {
                using var db = Database.Get();
                var votes = db.GetCollection<Vote>();

                var vote = currentUser.CreateVote(SelectedArticle);

                votes.Insert(vote);

                SelectedArticleVotes++;
            }, _ =>
            {
                return SelectedArticle != null;
            });

            ToggleFilterOwnCommand = new RelayCommand(_ =>
            {
                filterOwnFlag = !filterOwnFlag;

                ReloadArticles();

                FilterOwn();
                ReloadViewArticles();
            }, _ =>
            {
                using var db = Database.Get();
                return db.GetCollection<Article>()
                        .Query()
                        .Where(a =>
                            a.AuthorId == currentUser.Id
                        )
                        .Count() > 0;
            });

            SwitchSortNewestCommand = new RelayCommand(_ =>
            {
                sortNewestFlag = !sortNewestFlag;

                SortNewest();
                ReloadViewArticles();
            });

            SwitchSortBestCommand = new RelayCommand(_ =>
            {
                sortBestFlag = !sortBestFlag;

                SortBest();
                ReloadViewArticles();
            });
        }

        private void FilterOwn()
        {
            if (filterOwnFlag)
                articles = articles.Where(
                    a => a.AuthorId == currentUser.Id
                ).ToList();

            else
                articles = articles.Where(
                    a => a.AuthorId != currentUser.Id
                ).ToList();
        }

        private void SortNewest()
        {
            articles.Sort(delegate (Article first, Article second)
            {
                return second.PublicationTime.CompareTo(first.PublicationTime);
            });

            if (!sortNewestFlag)
                articles.Reverse();
        }

        private void SortBest()
        {
            Dictionary<Article, int> articleVotes = new Dictionary<Article, int>();

            using (var db = Database.Get())
            {
                var votes = db.GetCollection<Vote>();

                foreach (var article in articles)
                {
                    int count = votes.Query().Where(
                        v => v.ArticleId == article.Id
                    ).Count();

                    articleVotes.Add(article, count);
                }
            }

            var orderedArticles = articleVotes.OrderByDescending(a => a.Value);

            articles.Clear();

            foreach (var pair in orderedArticles)
                articles.Add(pair.Key);

            if (!sortBestFlag)
                articles.Reverse();
        }

        private void ReloadArticles()
        {
            articles.Clear();

            using (var db = Database.Get())
            {
                articles = db.GetCollection<Article>().FindAll().ToList();
            }
        }

        private void ReloadViewArticles()
        {
            ViewArticles.Clear();

            foreach (var article in articles)
                ViewArticles.Add(article);
        }
    }
}
