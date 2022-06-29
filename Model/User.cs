using Articles.Infrastructure;
using System;

namespace Articles.Model
{
    public class User : BaseNotifyPropertyChanged
    {
        private Guid id = Guid.NewGuid();

        private string name;

        public Guid Id
        {
            get => id;
            set
            {
                id = value;
                Notify();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                Notify();
            }
        }

        public User(string name) { Name = name; }

        public Article CreateArticle()
        {
            return new Article(Id);
        }

        public Article CreateArticle(string title, string text)
        {
            return new Article(Id) { Title = title, Text = text };
        }

        public Vote CreateVote(Article article)
        {
            return new Vote(article.Id, Id);
        }
    }
}
