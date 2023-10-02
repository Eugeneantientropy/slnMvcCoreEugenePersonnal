namespace prjMvcCoreEugenePersonnal.Models
{
    public class CPostWrap
    {
        private Post _post;
        public Post post { get { return _post; } set {  _post = value; } }
        public CPostWrap() 
        {
            _post = new Post(); 
        }
        public int PostId
        {
            get
            {
                return _post.PostId;
            }
            set
            {
                _post.PostId = value;   
            }
        }
        public int? UserId
        {
            get
            {
                return _post.UserId;
            }
            set
            {
                _post.UserId = value;
            }
        }
        public string Title
        {
            get
            {
                return _post.Title;
            }
            set
            {
                _post.Title = value;
            }
        }
        public string Content
        {
            get
            {
                return _post.Content;
            }
            set
            {
                _post.Content = value;
            }
        }
        public DateTime? DatePosted
        {
            get
            {
                return _post.DatePosted;
            }
            set
            {
                _post.DatePosted = value;
            }
        }
        public DateTime? LastEdited
        {
            get
            {
                return _post.LastEdited;
            }
            set
            {
                _post.LastEdited = value;
            }
        }
        public string PostImageUrl
        {
            get
            {
                return _post.PostImageUrl;
            }
            set
            {
                _post.PostImageUrl = value;
            }
        }
        public IFormFile Photo { get; set; }

    }
}
