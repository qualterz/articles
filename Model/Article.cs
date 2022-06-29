using Articles.Infrastructure;
using System;

namespace Articles.Model
{
    public class Article : BaseNotifyPropertyChanged
    {
        private DateTime publicationTime = DateTime.Now;
        private Guid id = Guid.NewGuid();
        private Guid authorId;

        private string title;
        private string content;

        public DateTime PublicationTime
        {
            get => publicationTime;
            set
            {
                publicationTime = value;
                Notify();
            }
        }

        public Guid Id
        {
            get => id;
            set
            {
                id = value;
                Notify();
            }
        }

        public Guid AuthorId
        {
            get => authorId;
            set
            {
                authorId = value;
                Notify();
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                Notify();
            }
        }
        public string Text
        {
            get => content;
            set
            {
                content = value;
                Notify();
            }
        }

        public Article(Guid authorId)
        {
            AuthorId = authorId;
        }
    }
}
