namespace FluentMvc.Spec
{
    using System.Web.Mvc;

    public class TestController : Controller
    {
        public ActionResult ReturnViewResult() { return View(); }
        public Post ReturnPost() { return new Post(); }
        public Post ReturnNull() { return null; }

        [HttpPost]
        public Post ReturnNull(Post post) { return null; }
    }

    public class Post
    {
    }
}