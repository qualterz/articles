using Articles.Infrastructure;
using System;

namespace Articles.Model
{
    public class Vote : BaseNotifyPropertyChanged
    {
        private Guid id = Guid.NewGuid();

        private Guid articleId;
        private Guid userId;

        public Guid Id
        {
            get => id;
            set
            {
                id = value;
                Notify();
            }
        }

        public Guid ArticleId
        {
            get => articleId;
            set
            {
                articleId = value;
                Notify();
            }
        }

        public Guid UserId
        {
            get => userId;
            set
            {
                userId = value;
                Notify();
            }
        }

        public Vote(Guid article, Guid voter)
        {
            ArticleId = article;
            UserId = voter;
        }
    }
}
