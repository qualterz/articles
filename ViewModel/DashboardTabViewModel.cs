using Articles.Infrastructure;
using Articles.Model;
using Articles.View;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Articles.ViewModel
{
    class DashboardTabViewModel : BaseNotifyPropertyChanged
    {
        private readonly User currentUser = LoginViewModel.Instance.User;

        private string prevArticleViewStateString;
        private string articleViewStateString;
        public string ArticleViewStateString
        {
            get => articleViewStateString;
            private set
            {
                prevArticleViewStateString = articleViewStateString;
                articleViewStateString = value;
                Notify();
            }
        }

        private ContentControl prevArticleView;
        private ContentControl articleView;
        public ContentControl ArticleView
        {
            get => articleView;
            private set
            {
                prevArticleView = articleView;
                articleView = value;
                Notify();
            }
        }

        public ObservableCollection<Article> UserArticles { get; set; }
            = new ObservableCollection<Article>();

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

        public Article CurrentArticle { get; set; }

        public ICommand NewArticleCommand { get; private set; }
        public ICommand EditArticleCommand { get; private set; }
        public ICommand DeleteArticleCommand { get; private set; }
        public ICommand ArticleChangedCommand { get; private set; }

        public DashboardTabViewModel()
        {
            InitializeCommands();

            ReloadUserArticles();

            NewArticleCommand.Execute(null);
        }

        private void ReloadUserArticles()
        {
            UserArticles.Clear();

            if (currentUser == null)
                return;

            using (var db = Database.Get())
            {
                var articles = db.GetCollection<Article>();
                var foundedUserArticles = articles.Query()
                    .Where(u => u.AuthorId == currentUser.Id).ToList();

                foreach (var article in foundedUserArticles)
                    UserArticles.Add(article);
            }
        }

        private void InitializeCommands()
        {
            NewArticleCommand = new RelayCommand(_ =>
            {
                ArticleViewStateString = "Editor";

                var control = new EditorControl();

                control.SubmitCommand = new RelayCommand(_ =>
                {
                    CurrentArticle
                        = LoginViewModel.Instance.User.CreateArticle(control.Title, control.Text);

                    UserArticles.Add(CurrentArticle);

                    using (var db = Database.Get())
                    {
                        var articles = db.GetCollection<Article>();
                        articles.Insert(CurrentArticle);
                    }

                    SelectedArticle = CurrentArticle;
                }, _ => { return control.Title?.Length > 0 && control.Text?.Length > 0; });

                control.CancelCommand = new RelayCommand(_ =>
                {
                    if (prevArticleView != null)
                    {
                        ArticleView = prevArticleView;
                        ArticleViewStateString = prevArticleViewStateString;
                    }
                }, _ => { return prevArticleView != null; });

                ArticleView = control;
            });

            EditArticleCommand = new RelayCommand(_ =>
            {
                if (!LoginViewModel.Instance.IsLogged)
                    return;

                if (SelectedArticle == null)
                    return;

                ArticleViewStateString = "Editor";

                var control = new EditorControl();

                control.Title = SelectedArticle.Title;
                control.Text = SelectedArticle.Text;

                control.SubmitCommand = new RelayCommand(_ =>
                {
                    using (var db = Database.Get())
                    {
                        var articles = db.GetCollection<Article>();

                        SelectedArticle.Title = control.Title;
                        SelectedArticle.Text = control.Text;

                        articles.Update(SelectedArticle);

                        ArticleChangedCommand.Execute(null);
                    }
                }, _ => { return control.Title?.Length > 0 && control.Text?.Length > 0; });

                control.CancelCommand = new RelayCommand(_ =>
                {
                    ArticleView = prevArticleView;
                    ArticleViewStateString = prevArticleViewStateString;
                }, _ => { return prevArticleView != null; });

                ArticleView = control;
            }, _ => { return SelectedArticle != null; });

            DeleteArticleCommand = new RelayCommand(_ =>
            {
                if (SelectedArticle == null)
                    return;

                using (var db = Database.Get())
                {
                    var articles = db.GetCollection<Article>();

                    articles.DeleteMany(a => a.Id == SelectedArticle.Id);

                    UserArticles.Remove(SelectedArticle);

                    NewArticleCommand.Execute(null);
                }
            }, _ => { return SelectedArticle != null; });

            ArticleChangedCommand = new RelayCommand(_ =>
            {
                ArticleViewStateString = "Preview";

                var control = new PreviewControl();

                control.Title = SelectedArticle.Title;
                control.Text = SelectedArticle.Text;

                ArticleView = control;
            });
        }
    }
}
